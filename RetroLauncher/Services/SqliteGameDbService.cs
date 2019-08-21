using Dapper;
using RetroLauncher.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Services
{
    /// <summary>
    /// База данных игр (SQLite)
    /// </summary>
    public class SqliteGameDbService : IGameDbService
    {
        private readonly string _connectionString = "Data Source=library_base.sqlite;Version=3;";        
               

        /// <summary>
        /// Получить одну игру по Id
        /// </summary>
        /// <returns></returns>
        public async Task<Game> GetGameByIdAsync(int gameId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //списко игр для уникальности
                var gameDictionary = new Dictionary<int, Game>();
                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, gb.annotation, 
                                pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.platform_alias as Alias, 
                                lnk.link_id as LinkId, lnk.url_alter as Url, lnk.type_url as TypeUrl
                            FROM rl_games_base gb
                            JOIN rl_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN rl_platforms pl ON gb.platform_id = pl.platform_id
                            JOIN rl_game_link lnk ON lnk.game_id = gb.game_id
                            WHERE gb.game_id = " + gameId.ToString();

                connection.Open();
                var games = await connection.QueryAsync<Game, Platform, GameLink, Game>
                    (sql, (game, platform, gamelink) =>
                    {
                        //проверям есть ли списка уже
                        if (!gameDictionary.TryGetValue(game.GameId, out var gameEntry))
                        {
                            gameEntry = game;
                            gameEntry.GameLinks = new List<GameLink>();
                            gameDictionary.Add(gameEntry.GameId, gameEntry);
                        }

                        gameEntry.Platform = platform;
                        gameEntry.GameLinks.Add(gamelink);
                        return gameEntry;
                    }, splitOn: "PlatformId,LinkId");
                return games.FirstOrDefault();
            }
        }
                
        public async Task<IEnumerable<string>> GetGenres()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {                
                var sql = @"SELECT genre_name FROM rl_genres";

                connection.Open();
                var genres = await connection.QueryAsync<string>(sql);
                return genres;
            }
        }

        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var sql = @"SELECT platform_id as PlatformId, platform_name as PlatformName, platform_alias as Alias FROM rl_platforms";

                connection.Open();
                var platforms = await connection.QueryAsync<Platform>(sql);
                return platforms;
            }
        }

        public async Task<(int, IEnumerable<Game>)> GetBase(int Count, int SkipCount)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {

                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, gb.annotation, 
                                pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.platform_alias as Alias, lnk.link_id as LinkId, lnk.url_alter as Url, lnk.type_url as TypeUrl
                            FROM rl_games_base gb
                            JOIN rl_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN rl_platforms pl ON gb.platform_id = pl.platform_id                            
                            left join (select lnks.* from rl_game_link lnks where lnks.type_url = 2) lnk on lnk.game_id = gb.game_id";
                try
                {

                    connection.Open();
                    var games = await connection.QueryAsync<Game, Platform, GameLink, Game>
                        (sql  + ("\n limit " + Count + " OFFSET " + SkipCount).ToString(), (game, platform, gamelink) =>
                        {
                            game.Platform = platform;
                            game.GameLinks = new List<GameLink>() { gamelink };
                            return game;
                        }, splitOn: "PlatformId, LinkId");

                    var count = await GetCount(sql, connection);
                    return (count, games);
                }
                catch (Exception e) { throw new Exception("Ошибка", e); }
            }
        }
             
        public async Task<(int, IEnumerable<Game>)> GetBaseFilter(int Count, int SkipCount, FilterGame filter)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {

                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, gb.annotation, 
                                pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.platform_alias as Alias, lnk.link_id as LinkId, lnk.url_alter as Url, lnk.type_url as TypeUrl
                            FROM rl_games_base gb
                            JOIN rl_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN rl_platforms pl ON gb.platform_id = pl.platform_id                            
                            left join (select lnks.* from rl_game_link lnks where lnks.type_url = 2) lnk on lnk.game_id = gb.game_id";

                //var filterSql = string.Format("WHERE (gb.game_name like '%{0}%' or gb.name_second like '%{0}%' or gb.name_other like '%{0}%' ) or (genre_name = '{1}') or (gb.platform_id = {2})", filter.Name, filter.Genre, filter.Platform.PlatformId);
                string filterSql = string.Empty;
                //словарь фильтров
                Dictionary<string, object> filters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(filter.Name))
                    filters["Name"] = filter.Name;
                if (!string.IsNullOrEmpty(filter.Genre))
                    filters["Genre"] = filter.Name;
                if (filter.Platform != null)
                    filters["Platform"] = filter.Platform;

                int i = 0;
                foreach(var dic in filters)
                {
                    switch(dic.Key)
                    {
                        case "Name":
                            if (i > 0) filterSql = " or " + filterSql;
                            filterSql += string.Format("(LOWER(gb.game_name) like '%{0}%' or LOWER(gb.name_second) like '%{0}%' or LOWER(gb.name_other) like '%{0}%' )", dic.Value);                            
                            break;
                        case "Genre":
                            if (i > 0) filterSql = " or " + filterSql;
                            filterSql += string.Format("(LOWER(genre_name) = '{0}')", dic.Value);
                            break;
                        case "Platform":
                            if (i > 0) filterSql = " or " + filterSql;
                            filterSql += string.Format("(gb.platform_id = {0})", (dic.Value as Platform).PlatformId);
                            break;
                    }
                    i++;
                }

                if (!string.IsNullOrEmpty(filterSql)) sql = sql + "\nWHERE " + filterSql;
                try
                {

                    connection.Open();
                    var games = await connection.QueryAsync<Game, Platform, GameLink, Game>
                        (sql + ("\n limit " + Count + " OFFSET " + SkipCount).ToString(), (game, platform, gamelink) =>
                        {
                            game.Platform = platform;
                            game.GameLinks = new List<GameLink>() { gamelink };
                            return game;
                        }, splitOn: "PlatformId, LinkId");

                    var count = await GetCount(sql, connection);
                    return (count, games);
                }
                catch (Exception e) { throw new Exception("Ошибка", e); }
            }
        }

        #region Count Method
        private async Task<int> GetCount(string querysql)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"SELECT COUNT(*) FROM (" + querysql + ")";
                connection.Open();
                int count = await connection.QueryFirstOrDefaultAsync<int>(sql);
                return count;
            }
        }

        private async Task<int> GetCount(string querysql, SQLiteConnection connection)
        {
            string sql = @"SELECT COUNT(*) FROM (" + querysql + ")";
            int count = await connection.QueryFirstOrDefaultAsync<int>(sql);
            return count;
        }
        #endregion
    }
}
