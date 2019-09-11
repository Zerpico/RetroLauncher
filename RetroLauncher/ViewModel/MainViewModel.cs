using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using RetroLauncher.Helpers;

namespace RetroLauncher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private RelayCommand _loadedCommand;
        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Home");
                        //RaisePropertyChanged("LoadedCommand");
                    }));
            }
        }

        private RelayCommand _recentCommand;
        public RelayCommand RecentCommand
        {
            get
            {
                return _recentCommand
                    ?? (_recentCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Recent");
                    }));
            }
        }

        private RelayCommand _downloadedCommand;
        public RelayCommand DownloadedCommand
        {
            get
            {
                return _downloadedCommand
                    ?? (_downloadedCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Downloaded");

                    }));
            }
        }

        private RelayCommand _navigateNextCommand;
        public RelayCommand NavigateNextCommand
        {
            get
            {
                return _navigateNextCommand
                    ?? (_navigateNextCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.GoBack();

                    }));
            }
        }

        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

    }
}
