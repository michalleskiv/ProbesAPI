using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProbesLib.Interfaces;

namespace ProbesLib.Models
{
    public class ProbesWorker : IProbesWorker
    {
        private static HttpClient _client;

        private readonly IConfig _config;

        public ProbesWorker(IConfig config)
        {
            _config = config;

            ConfigureClient();
        }

        public void ConfigureClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_config.Url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.Token);
        }

        public async Task<ResultDefinition> GetDefinition()
        {
            var url = _config.Url + _config.DefinitionsEndpoint
                .Replace("{idApp}", _config.IdApp)
                .Replace("{idSchema}", _config.IdSchema);

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStreamAsync();
                var resObject = await JsonSerializer.DeserializeAsync<ResultDefinition>(result);

                return resObject;
            }

            return null;
        }

        public async Task<ResultRecord> GetById(string idProbe)
        {
            var url = _config.Url + _config.ProbeEndpoint
                .Replace("{idApp}", _config.IdApp)
                .Replace("{idSchema}", _config.IdSchema)
                .Replace("{idProbe}", idProbe);

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStreamAsync();
                var resObject = await JsonSerializer.DeserializeAsync<ResultRecord>(result);

                return resObject;
            }

            return null;
        }

        public async Task<FilteredCount> ExecuteProbe(string idProbe)
        {
            var resRecord = await GetById(idProbe);

            var url = resRecord.data.fields.url;

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStreamAsync();

                var resObject = await JsonSerializer.DeserializeAsync<ResultCount>(result);

                return new FilteredCount
                {
                    respectiveValue = resObject.data.count,
                    respectiveTime = DateTime.Now
                };
            }

            return null;
        }
    }
}
