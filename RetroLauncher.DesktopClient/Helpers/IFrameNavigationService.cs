

namespace RetroLauncher.DesktopClient.Helpers
{
   /* public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
    */
    /// <summary>
    /// Интерфейс, определяющий навигацию между страницами
    /// </summary>
    public interface IFrameNavigationService
    {
        /// <summary>
        /// Ключ текущей страницы
        /// </summary>
        string CurrentPageKey
        {
            get;
        }

        /// <summary>
        /// Перейти на предыдущую страницу из стека.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Указывает сервису отобразить страницу по указаному ключу
        /// </summary>
        /// <param name="pageKey">Ключ отображаемой страницы</param>
        void NavigateTo(string pageKey);

        /// <summary>
        /// Указывает сервису отобразить страницу по указаному ключу и передать параметр
        /// </summary>
        /// <param name="pageKey">Ключ отображаемой страницы</param>
        /// <param name="parameter">Параметр для передачи</param>
        void NavigateTo(string pageKey, object parameter);

        void ShowWaitPage();
        void HideWaitPage();
        void LoadWaitPage();

        object Parameter { get; }
    }
}
