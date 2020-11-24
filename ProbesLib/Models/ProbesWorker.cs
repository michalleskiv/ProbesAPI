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

        public async Task<ProbesDTO> GetDefinition()
        {
            var probes = await GetAllProbes();

            return new ProbesDTO("1.0.0", probes);
        }

        public async Task<ProbeDTO> GetById(int idProbe)
        {
            var probes = await GetAllProbes();
            var probe = probes.SingleOrDefault(p => p.UniqueId == idProbe);

            return new ProbeDTO(probe);
        }

        public async Task<CountDTO> ExecuteProbe(int idProbe)
        {
            var probes = await GetAllProbes();
            var probe = probes.SingleOrDefault(p => p.UniqueId == idProbe);

            return new CountDTO
            {
                respectiveValue = await ExecuteQuery(probe),
                respectiveTime = DateTime.Now
            };
        }

        private async Task<List<Probe>> GetAllProbes()
        {
            var url = _config.Url + _config.Endpoint
                .Replace("{idApp}", _config.AppId)
                .Replace("{idSchema}", _config.TableId);

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

        private async Task<int> ExecuteQuery(Probe probe)
        {
            var url = _config.Url + _config.Endpoint
                .Replace("{idApp}", probe.AppId)
                .Replace("{idSchema}", probe.TableId);

            //var values

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
    }
}
