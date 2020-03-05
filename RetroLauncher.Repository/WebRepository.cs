using RetroLauncher.DAL.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Repository
{
    public class WebRepository : IRepository
    {
        private HttpClient client;
        private string APP_URL = "https://www.zerpico.ru/api/";  //todo: жестко привязно, исправить

        public WebRepository()
        {
            client = CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            // сначала создадим прокси
            var proxy = new WebProxy
            {
               // Address = new Uri($"http://{proxyHost}:{proxyPort}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false
                
               /* Credentials = new NetworkCredential(
                    userName: proxyUserName,
                    password: proxyPassword)*/
            };

            // Теперь клиентский обработчик, который использует этот прокси
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
            };

            // если нужно проходить аутентификацию на веб-сервере:
            /* if (needServerAuthentication)
             {
                 httpClientHandler.PreAuthenticate = true;
                 httpClientHandler.UseDefaultCredentials = false;

                 // *** These creds are given to the web server, not the proxy server ***
                 httpClientHandler.Credentials = new NetworkCredential(
                     userName: serverUserName,
                     password: serverPassword);
             }*/

            // создаем объект клиента HTTP
            return new HttpClient(handler: httpClientHandler, disposeHandler: true);
        }

        public async Task<Game> GetGameById(int gameId)
        {
            var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, $"games/{gameId}"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<Game>();
            }
            return null;
        }

        public async Task<PagingGames> GetGameFilter(string name = null, int[] genres = null, int[] platforms = null, int count = 50, int skip = 0)
        {
            //строка запроса к api
            string requestString = $"games?limit={count}&offset={skip}";

            //если выбрали имя для поиска
            if (name != null) requestString += $"&name={name}";

            //если выбрали жанры или платформы, то дописывает в строку эти выборки
            if (genres != null) requestString += "&"+GetStringFormatRequest(genres, "genres");
            if (platforms != null) requestString += "&" + GetStringFormatRequest(platforms, "platforms");

            //делаем запрос, получаем ответ
            var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, requestString));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<PagingGames>();
            }
            return new PagingGames() { Total = 0, Limit = 0, Offset = 0, Items = null };
        }

        public async Task<PagingGames> GetGames(int count=50, int skip=0)
        {
            var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, $"games?limit={count}&offset={skip}"));
            if (response.StatusCode == HttpStatusCode.OK)
            {              
                return await response.Content.ReadAsAsync<PagingGames> ();
            }
            return new PagingGames() { Total= 0, Limit=0, Offset= 0, Items = null};
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, "genres"));
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Moved)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Genre>>();
            }
            return null;
        }

        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, "platforms"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Platform>>();
            }
            return null;
        }

        /// <summary>
        /// Привести массив параметров к url запросу
        /// </summary>
        /// <param name="parameters">значения параметров</param>
        /// <param name="name">имя параметра</param>
        /// <returns></returns>
        private string GetStringFormatRequest(int[] parameters, string name)
        {
            string result = string.Empty;
            int i = 0;
            for (int j = 0; j < parameters.Count(); j++)
            {
                if (i > 0) result = result + "&";
                result += name + $"={parameters[j]}";
                i++;
            }
            return result;
        }
    }
}
