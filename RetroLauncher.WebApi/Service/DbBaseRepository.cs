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

        public Task<(int, IEnumerable<Game>)> GetBase(int Count, int SkipCount)
        {
            throw new NotImplementedException();
        }

        public async Task<(int, IEnumerable<Game>)> GetBaseFilter(FilterGame filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (filter.Count == 0 && filter.Skip == 0) return (0, new List<Game>());
                if (filter.Count > 1000) return (0, new List<Game>());

                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, down.Downloads, rat.Rating, 
                            pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.alias, lnk.link_id as LinkId, lnk.url, lnk.type_url as TypeUrl
                            FROM gb_games gb
                            JOIN gb_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN gb_platforms pl ON gb.platform_id = pl.platform_id                            
                            left join (select lnks.* from gb_links lnks where lnks.type_url = 2) lnk on lnk.game_id = gb.game_id
                            OUTER APPLY(SELECT COUNT(*) as Downloads FROM lg_downloads down WHERE down.game_id = gb.game_id) down                            
                            OUTER APPLY(SELECT cast(AVG(cast(rat.rating as numeric(18,8))) as numeric(18,2)) as rating FROM lg_ratings rat WHERE rat.game_id = gb.game_id) rat ";

                //var filterSql = string.Format("WHERE (gb.game_name like '%{0}%' or gb.name_second like '%{0}%' or gb.name_other like '%{0}%' ) or (genre_name = '{1}') or (gb.platform_id = {2})", filter.Name, filter.Genre, filter.Platform.PlatformId);
                string filterSql = string.Empty;
                //словарь фильтров
                Dictionary<string, object> filters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(filter.Name))
                    filters["Name"] = filter.Name;
                if (!string.IsNullOrEmpty(filter.Genre))
                    filters["Genre"] = filter.Name;
                if (filter.Platform != 0)
                    filters["Platform"] = filter.Platform;

                int i = 0;
                foreach (var dic in filters)
                {
                    switch (dic.Key)
                    {
                        case "Name":
                            if (i > 0) filterSql = " or " + filterSql;
                            filterSql += string.Format("(LOWER(gb.game_name) like '%{0}%' or LOWER(gb.name_second) like '%{0}%' or LOWER(gb.name_other) like '%{0}%' )", dic.Value);
                            break;
                        case "Genre":
                            if (i > 0) filterSql = " or " + filterSql;
                            filterSql += string.Format("(LOWER(gnr.genre_name) = '{0}')", dic.Value);
                            break;
                        case "Platform":
                            if (i > 0) filterSql = " or " + filterSql;
                            filterSql += string.Format("(gb.platform_id = {0})", dic.Value);
                            break;
                    }
                    i++;
                }

                if (!string.IsNullOrEmpty(filterSql)) sql = sql + "\nWHERE " + filterSql;
                try
                {
                    connection.Open();
                    var games = await connection.QueryAsync<Game, Platform, GameLink, Game>
                        (sql + ("\n ORDER BY GameId  \n OFFSET "+ filter.Skip + " ROWS \n FETCH NEXT "+ filter.Count + " ROWS ONLY").ToString(), (game, platform, gamelink) =>
                        {
                            game.Platform = platform;
                            gamelink.Url = "https://zerpico.ru/retrolauncher/" + gamelink.Url;
                            game.GameLinks = new List<GameLink>() { gamelink };
                            return game;
                        }, splitOn: "PlatformId, LinkId");

                    var count =  await GetCount(sql, connection);                  
                    return (count, games);
                }
                catch (Exception e) { throw new Exception("Ошибка", e); }
            }
        }

        public async Task<Game> GetGameById(int gameId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                //списко игр для уникальности
                var gameDictionary = new Dictionary<int, Game>();
                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, gb.annotation, 
                                pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.alias, 
                                lnk.link_id as LinkId, lnk.url, lnk.type_url as TypeUrl
                            FROM gb_games gb
                            JOIN gb_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN gb_platforms pl ON gb.platform_id = pl.platform_id
                            JOIN gb_links lnk ON lnk.game_id = gb.game_id
                            WHERE gb.game_id =  " + gameId.ToString();

                connection.Open();
                
                var games = await connection.QueryAsync<Game, Platform, GameLink, Game>
                    (sql, (game, platform, gamelink) =>
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
                        gameEntry.Platform = platform;
                        gameEntry.GameLinks.Add(gamelink);
                        return gameEntry;
                    }, splitOn: "PlatformId,LinkId");
                var result = (games != null && games.Count() > 0) ? games.FirstOrDefault() : new Game();
                return result;
            }
              //  return new Task<IGame>(() => { return new Game() { GameId = 1, Name = "sample" }; });
        }

        public async Task<IEnumerable<string>> GetGenres()
        {
            using (var connection = new SqlConnection(_connectionString))
            {                
                var sql = @"SELECT [genre_name] FROM [gb_genres]";

                connection.Open();
                var genres = await connection.QueryAsync<string>(sql);
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
        private async Task<int> GetCount(string querysql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT COUNT(*) FROM (" + querysql + ") T";
                connection.Open();
                int count = await connection.QueryFirstOrDefaultAsync<int>(sql);
                return count;
            }
        }

        private async Task<int> GetCount(string querysql, SqlConnection connection)
        {
            string sql = @"SELECT COUNT(*) FROM (" + querysql + ") T";
            int count = await connection.QueryFirstOrDefaultAsync<int>(sql);
            return count;
        }
        #endregion
    }
}
