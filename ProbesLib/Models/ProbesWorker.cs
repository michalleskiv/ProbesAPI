using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            _client = new HttpClient {BaseAddress = new Uri(_config.Url)};
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

            return null;
        }

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
            }

            return result;
        }

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
    }
}
