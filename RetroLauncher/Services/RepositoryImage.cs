using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Services
{
    static class RepositoryImage
    {
        private static readonly string Token = "AgAAAAAGq9GCAAXRAb30qVKOiEknksyK2vlHa2E";
        private static readonly string ApiHost = "https://cloud-api.yandex.net";
        private static readonly HttpClient _client;

        static RepositoryImage()
        {
            _client = CreateClient();
        }

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", Token);

            return client;
        }


        /// <summary>
        /// Получить прямую ссылку на файл диска
        /// </summary>
        /// <param name="file">Название файла</param>
        /// <returns>true если запрос выполнен успешно</returns>
        public static async Task<string> GetFileDirectUrl(string file)
        {
            using (var response = await _client.GetAsync(ApiHost + "/v1/disk/resources/download?path=RetroLauncherFiles/" + file))
            {
                var urlResult = await response.Content.ReadAsStringAsync();
                var loadjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(urlResult);

                return loadjson["href"];
            }
        }
    }
}
