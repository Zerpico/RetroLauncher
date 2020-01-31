using RetroLauncher.DAL.Service;
using RetroLauncher.DesktopClient.Helpers;
using RetroLauncher.DesktopClient.ViewModel.Base;

namespace RetroLauncher.DesktopClient.ViewModel
{
    public class RecentViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
       // private readonly IRepository _gameDb;

        public RecentViewModel(IFrameNavigationService navigationService, IRepository gameDb)
        {
            _navigationService = navigationService;

        }
    }
}