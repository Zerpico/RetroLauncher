using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using RetroLauncher.Model;

namespace RetroLauncher.Services
{
    class RepositoryBase
    {
        private static string _connectionString = "Data Source=library_base.sqlite;Version=3;";

        /// <summary>
        /// Получить компактный список игры, без привязки ссылок и аннотации
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Game>> GetGamesShortAsync()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //списко игр для уникальности               
                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, '' as annotation, 
                                pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.platform_alias as Alias
                            FROM rl_games_base gb
                            JOIN rl_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN rl_platforms pl ON gb.platform_id = pl.platform_id                            
                            limit 100";

                connection.Open();
                var games = await connection.QueryAsync<Game, Platform, Game>
                    (sql, (game, platform) =>
                    {
                        game.Platform = platform;                      
                        return game;
                    }, splitOn: "PlatformId");
                return games;
            }
        }

        /// <summary>
        /// Получить список игр 
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Game>> GetGamesAsync()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //списко игр для уникальности
                var gameDictionary = new Dictionary<int, Game>();
                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, gb.annotation, 
                                pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.platform_alias as Alias, lnk.link_id as LinkId, lnk.url_alter as Url, lnk.type_url as TypeUrl
                            FROM rl_games_base gb
                            JOIN rl_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN rl_platforms pl ON gb.platform_id = pl.platform_id
                            JOIN rl_game_link lnk ON lnk.game_id = gb.game_id
                            limit 10";

                connection.Open();
                var games = await connection.QueryAsync<Game, Platform, GameLink, Game>
                    (sql, (game, platform, gamelink) =>
                    {
                        //проверям есть ли списка уже
                        Game gameEntry;
                        if (!gameDictionary.TryGetValue(game.GameId, out gameEntry))
                        {
                            gameEntry = game;
                            gameEntry.GameLinks = new List<GameLink>();
                            gameDictionary.Add(gameEntry.GameId, gameEntry);
                        }

                        gameEntry.Platform = platform;
                        gameEntry.GameLinks.Add(gamelink);
                        return gameEntry;
                    }, splitOn: "PlatformId,LinkId");
                return games;


            }
        }


        /// <summary>
        /// Получить одну игру по Id
        /// </summary>
        /// <returns></returns>
        public static async Task<Game> GetGameByIdAsync(int gameId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //списко игр для уникальности
                var gameDictionary = new Dictionary<int, Game>();
                var sql = @"select gb.game_id as GameId, gb.game_name as Name, gb.name_second as NameSecond, gb.name_other as NameOther, gnr.genre_name as Genre, gb.year, gb.developer, gb.annotation, 
                                pl.platform_id as PlatformId, pl.platform_name as PlatformName, pl.platform_alias as Alias, lnk.link_id as LinkId, lnk.url_alter as Url, lnk.type_url as TypeUrl
                            FROM rl_games_base gb
                            JOIN rl_genres gnr ON gnr.genre_id = gb.genre_id
                            JOIN rl_platforms pl ON gb.platform_id = pl.platform_id
                            JOIN rl_game_link lnk ON lnk.game_id = gb.game_id
                            WHERE gb.game_id = "+gameId.ToString();

                connection.Open();
                var games = await connection.QueryAsync<Game, Platform, GameLink, Game>
                    (sql, (game, platform, gamelink) =>
                    {
                        //проверям есть ли списка уже
                        Game gameEntry;
                        if (!gameDictionary.TryGetValue(game.GameId, out gameEntry))
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

    }
}
