using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Input;
using RetroLauncher.Helpers;
using RetroLauncher.ViewModel.Base;

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
                        if (System.IO.Directory.Exists(Service.Storage.Source.PathEmulator))
                        {
                            _navigationService.NavigateTo("Home");
                        }
                        else
                        {
                            _navigationService.LoadWaitPage();
                            loadEmulator();
                        }


                    }));
            }
        }

        private void loadEmulator()
        {
            var progress = new Progress<(int progress, string bytes)>(
             (value) =>
             {
                 MessengerInstance.Send((value.progress, "Загрузка эмулятора"));
                 if (value.progress >= 100)
                 {
                     _navigationService.NavigateTo("Home");
                 }
             });
            var fileDownloader = new Service.FileDownloader(progress);
            fileDownloader.DownloadFile("http://www.zerpico.ru/retrolauncher/mednafen.zip", @"C:\TMP\mednafen.zip");




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


        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

    }
}
