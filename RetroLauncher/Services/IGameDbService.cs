using RetroLauncher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Services
{
    /// <summary>
    /// База данных игр
    /// </summary>
    public interface IGameDbService
    {
        /// <summary>
        /// Получить компактный список игры, без привязки ссылок и аннотации
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Game>> GetGamesShortAsync();

        /// <summary>
        /// Получить список игр 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Game>> GetGamesAsync();

        /// <summary>
        /// Получить одну игру по Id
        /// </summary>
        /// <returns></returns>
        Task<Game> GetGameByIdAsync(int gameId);
    }
}
