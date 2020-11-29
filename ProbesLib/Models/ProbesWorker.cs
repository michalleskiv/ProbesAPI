using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProbesLib.Configurations;
using ProbesLib.Data.Count;
using ProbesLib.Data.DTO;
using ProbesLib.Data.Exceptions;
using ProbesLib.Data.Record;
using ProbesLib.Interfaces;

namespace ProbesLib.Models
{
    /// <summary>
    /// Implements the functionality of IProbesWorker
    /// </summary>
    public class ProbesWorker : IProbesWorker
    {
        private HttpClient _client;

        private readonly IConfig _config;

        public ProbesWorker(IConfig config)
        {
            _config = config;

            ConfigureClient();
        }

        public void ConfigureClient()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.Token);
        }

        public string Bonjour()
        {
            return File.ReadAllText("bonjour.json");
        }

        public async Task<DtoProbes> GetDefinition()
        {
            var probes = await GetAllProbes();

            return new DtoProbes("1.0.0", probes);
        }

        public async Task<DtoProbe> GetById(int idProbe)
        {
            return new DtoProbe(await GetOneProbe(idProbe));
        }

        public async Task<DtoCount> ExecuteProbe(int idProbe)
        {
            var probe = await GetOneProbe(idProbe);

            return new DtoCount
            {
                RespectiveValue = await ExecuteQuery(probe),
                RespectiveTime = DateTime.Now
            };
        }

        /// <summary>
        /// Gets all probes from database
        /// </summary>
        /// <returns>All probes</returns>
        private async Task<List<Probe>> GetAllProbes()
        {
            var url = _config.Url + _config.Endpoint
                .Replace("{appId}", _config.AppId)
                .Replace("{tableId}", _config.TableId);

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resObject = JsonConvert.DeserializeObject<ResultRecord>(result);

                var probes = resObject.Data.Select(r => r.Fields).ToList();

                return probes;
            }

            return new List<Probe>();
        }

        /// <summary>
        /// Gets probes by "uniqueId" field
        /// </summary>
        /// <param name="uniqueId">Probe uniqueId</param>
        /// <returns>A certain probe</returns>
        private async Task<Probe> GetOneProbe(int uniqueId)
        {
            var url = _config.Url + _config.FindProbeEndpoint
                .Replace("{appId}", _config.AppId)
                .Replace("{tableId}", _config.TableId)
                .Replace("{uniqueId}", uniqueId.ToString());

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resObject = JsonConvert.DeserializeObject<ResultRecord>(result);

                var probes = resObject.Data.Select(r => r.Fields).ToList();

                try
                {
                    return probes.Single(p => p.UniqueId == uniqueId);
                }
                catch (Exception)
                {
                    throw new ProbeNotFoundException(uniqueId, url, response);
                }
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new DrMaxServerErrorException(HttpStatusCode.Conflict);
            }

            throw new UnknownException(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Executes probe
        /// </summary>
        /// <param name="probe">Probe to execute</param>
        /// <returns>Count of found items</returns>
        private async Task<int> ExecuteQuery(Probe probe)
        {
            int result = 0;

            switch (probe.Type)
            {
                case "UserFilter":
                    result = await ExecuteUserFilter(probe);
                    break;
                case "API":
                    result = await ExecuteApiFilter(probe);
                    break;
                case "Magento":
                    result = await ExecuteMagento(probe);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Executes probe with API type
        /// </summary>
        /// <param name="probe">Probe with API type</param>
        /// <returns>Count of found items</returns>
        private async Task<int> ExecuteApiFilter(Probe probe)
        {
            var url = _config.Url + _config.ProbeEndpoint
                .Replace("{appId}", probe.AppId)
                .Replace("{tableId}", probe.TableId);

            var content = new StringContent(probe.FilterBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resObject = JsonConvert.DeserializeObject<ResultCount>(result);

                return resObject.Data.Count;
            }

            return default;
        }

        /// <summary>
        /// Executes probe with User Filter type
        /// </summary>
        /// <param name="probe">Probe with User Filter type</param>
        /// <returns>Count of found items</returns>
        private async Task<int> ExecuteUserFilter(Probe probe)
        {
            var url = _config.Url + _config.FilterEndpoint
                .Replace("{appId}", probe.AppId)
                .Replace("{tableId}", probe.TableId)
                .Replace("{filter}", probe.UserFilter);

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resObject = JsonConvert.DeserializeObject<ResultCount>(result);

                return resObject.Data.Count;
            }

            return default;
        }

        /// <summary>
        /// Executes probe with Magento type
        /// </summary>
        /// <param name="probe">Probe with Magento type</param>
        /// <returns>Count of found items</returns>
        private async Task<int> ExecuteMagento(Probe probe)
        {
            using var client = new HttpClient();

            var url = SetCurrentPageAndSize(probe.Url);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", probe.ApiKey.Fields.Value);
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var rss = JObject.Parse(result);

                return (int) rss["total_count"];
            }

            return default;
        }

        /// <summary>
        /// Adds pageSize and currentPage parameters if they were missed
        /// </summary>
        /// <param name="url">Current URL</param>
        /// <returns>URL with required parameters</returns>
        private string SetCurrentPageAndSize(string url)
        {
            var queryString = HttpUtility.ParseQueryString(url);

            var pageSize = queryString["searchCriteria[pageSize]"];
            if (pageSize == null)
            {
                queryString.Add("searchCriteria[pageSize]", "1");
            }

            var currentPage = queryString["searchCriteria[currentPage]"];
            if (currentPage == null)
            {
                queryString.Add("searchCriteria[currentPage]", "1");
            }

            return queryString.ToString();
        }
    }
}
