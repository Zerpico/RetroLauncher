using RetroLauncher.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Data.Service
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
        Task<IEnumerable<string>> GetGenres();

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

        /// <summary>
        /// Получить список игр выбраного количество 
        /// </summary>
        /// <param name="Count">количество выводимых игр</param>
        /// <param name="SkipCount">размер сдвига</param>
        /// <returns>возвращает общее количество игр без сдвига и список игр</returns>
        Task<(int, IEnumerable<Game>)> GetBase(int Count, int SkipCount);

        /// <summary>
        /// Поиск игр по названию, платформе, жанру
        /// </summary>
        /// <param name="Count">количество выводимых игр</param>
        /// <param name="SkipCount">размер сдвига</param>
        /// <param name="name">название игры</param>
        /// <param name="platform">платформа</param>
        /// <param name="genre">жанр</param>
        /// <returns>возвращает количество игр без сдвига и список игр</returns>
        Task<(int, IEnumerable<Game>)> GetBaseFilter(FilterGame filters);
    }
}
