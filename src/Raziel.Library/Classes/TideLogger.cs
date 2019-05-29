using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Raziel.Library.Models;

namespace Raziel.Library.Classes
{
    public class TideLogger : ITideLogger
    {
        private readonly LoggerSettings _settings;

        public TideLogger(Settings settings) {
            _settings = settings.LoggerSettings;

        }
        public async void Log(TideLog log) {
            log.Identifier = _settings.Identifier;
            var json = JsonConvert.SerializeObject(log);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient(){BaseAddress = new Uri(_settings.Connection)})
            {
                // Error here
                var httpResponse = await httpClient.PostAsync("/log", httpContent);
                if (httpResponse.Content != null)
                {
                    // Error Here
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
