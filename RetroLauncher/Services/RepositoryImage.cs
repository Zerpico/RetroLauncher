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
    class RepositoryImage
    {
        static string token = "AgAAAAAGq9GCAAXRAb30qVKOiEknksyK2vlHa2E";
        static string  APP_PATH = "https://cloud-api.yandex.net";

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))            
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
            
            return client;
        }


        /// <summary>
        /// Получить прямую ссылку на файл диска
        /// </summary>
        /// <param name="file">Название файла</param>
        /// <returns>true если запрос выполнен успешно</returns>
        public static async Task<string> GET(string file)
        {
            using (var client = CreateClient())
            {
                var response = await client.GetAsync(APP_PATH + "/v1/disk/resources/download?path=RetroLauncherFiles/" + file);
                var urlResult = await response.Content.ReadAsStringAsync();
                var loadjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(urlResult);
                return loadjson["href"];
            }
        }
    }
}
