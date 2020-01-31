using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RetroLauncher.Common;
using RetroLauncher.DAL.Model;
using RetroLauncher.DAL.Service;

namespace RetroLauncher.DAL.Repository
{
    public class WebRestRepository : IRepository
    {
        const string url = "https://www.zerpico.ru/api";//"https://localhost:5001/api";//"https://www.zerpico.ru/api";
        HttpClient _client;

        public WebRestRepository()
        {
            _client = CreateClient();
        }

        /// <summary>
        /// создать новый http клиент
        /// </summary>
        /// <returns></returns>
        private HttpClient CreateClient()
        {

            HttpClientHandler  httpClientHandler;
            //сначала определем тип прокси
            TypeProxy typeProxy = Storage.Source.GetValue<TypeProxy>("ProxyType");


            if (typeProxy != TypeProxy.Default)
            {
                //определим настройки для прокси
                WebProxy proxy = new WebProxy();

                string proxyHost = Storage.Source.GetValue("ProxyHost").ToString();
                int proxyPort = Storage.Source.GetValue<int>("ProxyPort");
                proxy.Address = new Uri($"http://{proxyHost.Trim()}:{proxyPort.ToString()}");
                proxy.BypassProxyOnLocal = false;
                proxy.UseDefaultCredentials = false;

                  /*Credentials = new NetworkCredential(
                    userName: proxyUserName,
                    password: proxyPassword)*/

                httpClientHandler = new HttpClientHandler { Proxy = proxy };
            }
            else
            {
                httpClientHandler = new HttpClientHandler();
                httpClientHandler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
            }





            // Omit this part if you don't need to authenticate with the web server:
        /*     if (needServerAuthentication)
            {
                httpClientHandler.PreAuthenticate = true;
                httpClientHandler.UseDefaultCredentials = false;

                // *** These creds are given to the web server, not the proxy server ***
                httpClientHandler.Credentials = new NetworkCredential(
                    userName: serverUserName,
                    password: serverPassword);
            }*/

            // Finally, create the HTTP client object
            return new HttpClient(handler: httpClientHandler, disposeHandler: true)  { Timeout = TimeSpan.FromSeconds(10) };
        }

        public Task<(int, IEnumerable<IGame>)> GetBase(int Count, int SkipCount)
        {
            throw new NotImplementedException();
        }

        public async Task<(int, IEnumerable<IGame>)> GetBaseFilter(FilterGame filter)
        {
           // List<IGame> games = new List<Model.Game>();
            try
            {
                //словарь фильтров
                Dictionary<string, object> filters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(filter.Name))
                    filters["Name"] = filter.Name;
                if (filter.Genre != null && filter.Genre.Count() > 0)
                    filters["Genre"] = filter.Genre;
                if (filter.Platform != null && filter.Platform.Count() > 0)
                    filters["Platform"] = filter.Platform;
                filters["Count"] = filter.Count.ToString();
                filters["Skip"] = filter.Skip.ToString();


                    //составляем строку параметров
                    string parameters = string.Empty;
                    int i = 0;
                    foreach (var dic in filters)
                    {
                        switch (dic.Key)
                        {
                            case nameof(FilterGame.Name):
                                if (i > 0) parameters =  parameters + "&";
                                parameters += nameof(FilterGame.Name) + $"={dic.Value}";
                                break;
                            case nameof(FilterGame.Genre):
                                int[] genres = (dic.Value as int[]);
                                for (int j = 0; j < genres.Count(); j++)
                                {
                                    if (i > 0) parameters = parameters + "&";
                                    parameters += nameof(FilterGame.Genre) + $"={genres[j]}";
                                    i++;
                                }
                                break;
                            case nameof(FilterGame.Platform):
                                int[] platforms = (dic.Value as int[]);
                                for (int j = 0; j < platforms.Count(); j++)
                                {
                                    if (i > 0) parameters = parameters + "&";
                                    parameters += nameof(FilterGame.Platform) + $"={platforms[j]}";
                                    i++;
                                }
                                break;
                            case nameof(FilterGame.Count):
                                if (i > 0) parameters =  parameters + "&";
                                parameters += nameof(FilterGame.Count) + $"={dic.Value}";
                                break;
                            case nameof(FilterGame.Skip):
                                if (i > 0) parameters =  parameters + "&";
                                parameters += nameof(FilterGame.Skip) + $"={dic.Value}";
                                break;
                        }
                        i++;
                    }

                    var response = await _client.GetAsync(url+@"/games/getfilter?" +parameters);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result =  JsonConvert.DeserializeObject<(int, IEnumerable<Game>)>(content);
                        return result;
                    }

            }
            catch (Exception e) { throw new Exception("Не удалось получить данные:.\n"+e.ToString());  }
            return (0, null);
        }

        public async Task<IGame> GetGameById(int gameId)
        {
            IGame game = new Game();
            try
            {

                var response = await _client.GetAsync(url + $"/games/{gameId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    game = JsonConvert.DeserializeObject<Game>(content);
                }

            }
            catch (Exception e) { throw new Exception("Не удалось получить данные:.\n" + e.ToString()); }
            return game;
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            IEnumerable<Genre> genres = new List<Genre>();


                var response = await _client.GetAsync(url +@"/games/getgenres");
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    genres = JsonConvert.DeserializeObject<IEnumerable<Genre>>(content);
                }

            return genres;
        }

        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            IEnumerable<Platform> platforms = new List<Platform>();


                var response = await _client.GetAsync(url +@"/games/getplatforms");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    platforms = JsonConvert.DeserializeObject<IEnumerable<Platform>>(content);
                }

            return platforms;
        }
    }
}
