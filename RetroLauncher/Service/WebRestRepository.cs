using Newtonsoft.Json;
using RetroLauncher.Data.Model;
using RetroLauncher.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Service
{
    public class WebRestRepository : IRepository
    {
        public Task<(int, IEnumerable<Game>)> GetBase(int Count, int SkipCount)
        {
            throw new NotImplementedException();
        }

        public async Task<(int, IEnumerable<Game>)> GetBaseFilter(FilterGame filter)
        {
            List<Game> games = new List<Game>();
            try
            {
                //словарь фильтров
                Dictionary<string, string> filters = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(filter.Name))
                    filters["Name"] = filter.Name;
                if (!string.IsNullOrEmpty(filter.Genre))
                    filters["Genre"] = filter.Name;
                if (filter.Platform != 0)
                    filters["Platform"] = filter.Platform.ToString();
                if (filter.Count != 0)
                    filters["Count"] = filter.Count.ToString();
                if (filter.Skip != 0)
                    filters["Skip"] = filter.Skip.ToString();

                using (HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                {
                    //состовляем строку параметров
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
                                if (i > 0) parameters =  parameters + "&";
                                parameters += nameof(FilterGame.Genre) + $"={dic.Value}";
                                break;
                            case nameof(FilterGame.Platform):
                                if (i > 0) parameters =  parameters + "&";
                                parameters += nameof(FilterGame.Platform) + $"={dic.Value}";
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

                    var response = await client.GetAsync(@"https://www.zerpico.ru/api/games/getfilter?"+parameters);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result =  JsonConvert.DeserializeObject<(int, IEnumerable<Game>)>(content);
                        return result;
                    }
                }
            }
            catch (Exception e) { throw new Exception("Не удалось получить данные:.\n"+e.ToString());  }
            return (0, null);
        }

        public async Task<Game> GetGameById(int gameId)
        {
            Game game = new Game();
            try
            {
                using (HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                {

                    var response = await client.GetAsync($"https://www.zerpico.ru/api/games/{gameId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        game = JsonConvert.DeserializeObject<Game>(content);
                    }
                }
            }
            catch(Exception e) { throw new Exception("Не удалось получить данные:.\n"+e.ToString());  }
            return game;
        }

        public async Task<IEnumerable<string>> GetGenres()
        {
            IEnumerable<string> genres = new List<string>();
            using (HttpClient client = new HttpClient())
            {

                var response = await client.GetAsync(@"https://www.zerpico.ru/api/games/getgenres");
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    genres = JsonConvert.DeserializeObject<IEnumerable<string>>(content);
                }
            }
            return genres;
        }

        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            IEnumerable<Platform> platforms = new List<Platform>();
            using (HttpClient client = new HttpClient())
            {

                var response = await client.GetAsync(@"https://www.zerpico.ru/api/games/getplatforms");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    platforms = JsonConvert.DeserializeObject<IEnumerable<Platform>>(content);
                }
            }
            return platforms;
        }
    }
}
