using RetroLauncher.DAL.Model;
using RetroLauncher.DAL.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RetroLauncher.DAL
{/*
    public class SQLServerRepository : IRepository
    {
        SqlServerConnection _sqlConnection;
        public SQLServerRepository(string connectionString)
        {
            _sqlConnection = SqlServerConnection.Instance;
            _sqlConnection.Add("LocalConnection", connectionString);
            _sqlConnection.Init("LocalConnection");
        }

        public Task<IEnumerable<Game>> GetBaseFilter(string Name, int[] Genre, int[] Platform, int OrderByName, int OrderByPlatform, int OrderByRating, int OrderByDownload)
        {
            throw new NotImplementedException();
        }

        public async Task<Game> GetGameById(int gameId)
        {


            //списко игр для уникальности
            var gameDictionary = new Dictionary<int, Game>();
            var sql = string.Format("SELECT * FROM GetGameById({0})", gameId);

            _sqlConnection.MainBaseConnection.Open();

            var games = await _sqlConnection.MainBaseConnection.QueryAsync<Game, Platform, Genre, GameLink, Game>
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
            _sqlConnection.MainBaseConnection.Close();
            return result;

        }

        public async Task<IEnumerable<Game>> GetGames()
        {
            var sql = @"SELECT game_id as GameId, game_name Name, name_second as NameSecond, name_other as NameOther, [Year], [Developer],[Annotation]
	                        ,g.genre_id as GenreId, genre_name as GenreName
	                        ,pl.platform_id as PlatformId, Platform_Name as PlatformName, Alias
                        FROM [dbo].[gb_games] gb
                        JOIN gb_genres g ON gb.genre_id = g.genre_id
                        JOIN gb_platforms pl ON gb.platform_id = pl.platform_id
                        ORDER BY [game_id] DESC
                        ";

            _sqlConnection.MainBaseConnection.Open();

            var games = await _sqlConnection.MainBaseConnection.QueryAsync<Game, Platform, Genre, Game>
                (sql, (game, platform, genre) =>
                {
                    game.Genre = genre;
                    game.Platform = platform;
                    return game;
                }, splitOn: "GenreId,PlatformId");

            _sqlConnection.MainBaseConnection.Close();
            return games;
        }

        public Task<IEnumerable<Genre>> GetGenres()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Platform>> GetPlatforms()
        {
            throw new NotImplementedException();
        }
    }*/
}
