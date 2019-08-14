using System.Threading.Tasks;

namespace RetroLauncher.Services
{
    /// <summary>
    /// Сервис для получения прямых ссылок по названию файла
    /// </summary>
    public interface IFileUrlService
    {
        /// <summary>
        /// Получить прямую ссылку на файл диска
        /// </summary>
        /// <param name="file">Название файла</param>
        /// <returns>true если запрос выполнен успешно</returns>
        Task<string> GetFileDirectUrl(string file);
    }
}
