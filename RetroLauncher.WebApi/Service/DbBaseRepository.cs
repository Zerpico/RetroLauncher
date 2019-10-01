using Microsoft.Extensions.Configuration;
using RetroLauncher.Data.Model;
using RetroLauncher.Data.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using RetroLauncher.Data;
using RetroLauncher.WebApi.Model;

namespace RetroLauncher.WebApi.Service
{
    public class DbBaseRepository : IRepository
    {
        public static IConfigurationRoot Configuration;

        private static string _connectionString
        {
            get
            {
                var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
                var connectionString = Configuration["ConnectionStrings:GamesLibraryConnection"];
                return connectionString;
            }
        }
              

        public async Task<(int, IEnumerable<IGame>)> GetBaseFilter(FilterGame filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (filter.Count == 0 && filter.Skip == 0) return (0, new List<Game>());
                if (filter.Count > 1000) return (0, new List<Game>());

                /*
                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, down.Downloads, rat.Rating, 
                            pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.alias, lnk.link_id as LinkId, lnk.url, lnk.type_url as TypeUrl
                            FROM gb_games gb
                            JOIN gb_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN gb_platforms pl ON gb.platform_id = pl.platform_id                            
                            left join (select lnks.* from gb_links lnks where lnks.type_url = 2) lnk on lnk.game_id = gb.game_id
                            OUTER APPLY(SELECT COUNT(*) as Downloads FROM lg_downloads down WHERE down.game_id = gb.game_id) down                            
                            OUTER APPLY(SELECT cast(AVG(cast(rat.rating as numeric(18,8))) as numeric(18,2)) as rating FROM lg_ratings rat WHERE rat.game_id = gb.game_id) rat ";
*/
                string genres = "", platforms = "";

                if (filter.Genre != null)
                    for (int i = 0; i < filter.Genre.Count(); i++)
                        genres += filter.Genre[i] + ",";

                if (filter.Platform != null)
                    for (int i = 0; i < filter.Platform.Count(); i++)
                        platforms += filter.Platform[i] + ",";

                var sql = string.Format("SELECT * FROM dbo.GetFilterGames({0}, {1}, '{2}', '{3}', '{4}', {5}, {6}, {7}, {8})",
                    filter.Count,
                    filter.Skip,
                    filter.Name,
                    genres,
                    platforms,
                    filter.OrderByName,
                    filter.OrderByPlatform,
                    filter.OrderByRating,
                    filter.OrderByDownload);
                
                
                try
                {
                    connection.Open();
                    var games = await connection.QueryAsync<Game, Platform, Genre, GameLink, Game>
                        (sql, (game, platform, genre, gamelink ) =>
                        {
                            game.Genre = genre;
                            game.Platform = platform;
                            gamelink.Url = "https://zerpico.ru/retrolauncher/" + gamelink.Url;
                            game.GameLinks = new List<GameLink>() { gamelink };
                            return game;
                        }, splitOn: "PlatformId, GenreId, LinkId");

                    var count =  await GetCount(filter, connection);                  
                    return (count, games);
                }
                catch (Exception e) { throw new Exception("Ошибка", e); }
            }
        }

        public async Task<IGame> GetGameById(int gameId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                //списко игр для уникальности
                var gameDictionary = new Dictionary<int, Game>();
                var sql = string.Format("SELECT * FROM GetGameById({0})", gameId);

                connection.Open();
                
                var games = await connection.QueryAsync<Game, Platform, Genre, GameLink, Game>
                    (sql, (game, platform, genre, gamelink) =>
                    {
                        //проверям есть ли списка уже
                        if (!gameDictionary.TryGetValue(game.GameId, out var gameEntry))
                        {
                            GameLink g = new GameLink();
                            gameEntry = game;
                            gameEntry.GameLinks = new List<GameLink>();
                            gameDictionary.Add(gameEntry.GameId, gameEntry);
                        }
                        gamelink.Url = "https://zerpico.ru/retrolauncher/" + gamelink.Url;
                        gameEntry.Genre = genre;
                        gameEntry.Platform = platform;
                        gameEntry.GameLinks.Add(gamelink);
                        return gameEntry;
                    }, splitOn: "PlatformId,GenreId,LinkId");
                var result = (games != null && games.Count() > 0) ? games.FirstOrDefault() : new Game();
                return result;
            }
              //  return new Task<IGame>(() => { return new Game() { GameId = 1, Name = "sample" }; });
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            using (var connection = new SqlConnection(_connectionString))
            {                
                var sql = @"SELECT genre_id as GenreId, genre_name as GenreName FROM gb_genres";

                connection.Open();
                var genres = await connection.QueryAsync<Genre>(sql);
                return genres;
            }
        }

        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            using (var connection = new SqlConnection(_connectionString))
            {                
                var sql = @"SELECT [platform_id] as PlatformId,[platform_name] as PlatformName,[alias] FROM [gb_platforms]";

                connection.Open();
                var platforms = await connection.QueryAsync<Platform>(sql);
                return platforms;
            }
        }



        #region Count Method
        private async Task<int> GetCount(FilterGame filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string genres = "", platforms = "";

                if (filter.Genre != null)
                    for (int i = 0; i < filter.Genre.Count(); i++)
                        genres += filter.Genre[i] + ",";

                if (filter.Platform != null)
                    for (int i = 0; i < filter.Platform.Count(); i++)
                        platforms += filter.Platform[i] + ",";

                var sql = string.Format("SELECT dbo.GetFilterMaxCountGames('{0}','{1}','{2}')",
                    filter.Name,
                    genres,
                    platforms);
                int count = await connection.QueryFirstOrDefaultAsync<int>(sql);
                return count;
            }
        }

        private async Task<int> GetCount(FilterGame filter, SqlConnection connection)
        {
            string genres = "", platforms = "";

            if (filter.Genre != null)
                for (int i = 0; i < filter.Genre.Count(); i++)
                    genres += filter.Genre[i] + ",";

            if (filter.Platform != null)
                for (int i = 0; i < filter.Platform.Count(); i++)
                    platforms += filter.Platform[i] + ",";

            var sql = string.Format("SELECT dbo.GetFilterMaxCountGames('{0}','{1}','{2}')",                   
                    filter.Name,
                    genres,
                    platforms );            
            int count = await connection.QueryFirstOrDefaultAsync<int>(sql);
            return count;
        }
                
        #endregion
    }
}
