using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RetroLauncher.Services
{

    /// <summary>
    /// Сервис для получения прямых ссылок по названию файла из ЯДиска
    /// </summary>
    public class YadiskFileUrlService : IFileUrlService
    {
        private static readonly string Token = "AgAAAAAGq9GCAAXRAb30qVKOiEknksyK2vlHa2E";
        private static readonly string ApiHost = "https://cloud-api.yandex.net";
        private readonly HttpClient _client;

        public YadiskFileUrlService()
        {
            _client = CreateClient();
        }

        /// <inheritdoc />
        public async Task<string> GetFileDirectUrl(string file)
        {
            using (var response = await _client.GetAsync(ApiHost + "/v1/disk/resources/download?path=RetroLauncherFiles/" + file))
            {
                var urlResult = await response.Content.ReadAsStringAsync();
                var loadjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(urlResult);

                return loadjson["href"];
            }
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", Token);

            return client;
        }
    }
}
