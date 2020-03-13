namespace RetroLauncher.ServiceTools.Download
{
    public enum DownloadStatus
    {
        // Значение по умолчанию
        Initialized,

        // Ожидает загрузки файла
        Waiting,

        // Скачивание с сервера
        Downloading,
        Deleting,

        // Загрузка добавлена в очередь
        Queued,

        // Загрузка выполнена
        Completed,

        // Во время загрузки произошла ошибка
        Error
    }
}