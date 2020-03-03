using RetroLauncher.DAL.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetroLauncher.DAL.Service
{
    /// <summary>
    /// База данных игр
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Получить список жанров
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetGenres();

        /// <summary>
        /// Получить список платформ
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Platform>> GetPlatforms();

        /// <summary>
        /// Получить одну игру по Id
        /// </summary>
        /// <returns></returns>
        Task<Game> GetGameById(int gameId);

        Task<PagingGames> GetGames(int count, int skip);

        Task<PagingGames> GetGameFilter(string name, int[] genres, int[] platforms, int count, int skip);
            
    }
}
