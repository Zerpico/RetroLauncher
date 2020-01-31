using System;
using RetroLauncher.Common;
using RetroLauncher.DesktopClient.Helpers;
using RetroLauncher.DesktopClient.ViewModel.Base;

namespace RetroLauncher.DesktopClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private RelayCommand _loadedCommand;
        private int _lastPercent;

        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand ??= new RelayCommand(() =>
                {
                    if (System.IO.Directory.Exists(Service.Storage.Source.PathEmulator))
                        _navigationService.NavigateTo("Home");
                    else
                    {
                        _navigationService.LoadWaitPage();
                        LoadEmulator();

                    }
                });
            }
        }

        

        private void LoadEmulator()
        {
            var progress = new Progress<(int progress, string bytes)>
            (
                (value) =>
                {
                    if (value.progress != _lastPercent)
                    {
                        this.MessengerInstance.Send<ProgressMessage, int>(null);
                        MessengerInstance.Send(new ProgressMessage()
                            {Percent = value.progress, Message = "Загрузка эмулятора"});
                        _lastPercent = value.progress;

                    }

                    if (value.progress >= 100)
                    {
                        ArchiveExtractor.ExtractAll(
                            System.IO.Path.Combine(Service.Storage.Source.PathApp, "mednafen.zip"),
                            Service.Storage.Source.PathEmulator);
                        System.IO.File.Delete(System.IO.Path.Combine(Service.Storage.Source.PathApp, "mednafen.zip"));

                        _navigationService.NavigateTo("Home");
                    }
                });
            var fileDownloader = new Service.FileDownloader(progress);
            fileDownloader.DownloadFile("https://www.zerpico.ru/retrolauncher/mednafen.zip",
                System.IO.Path.Combine(Service.Storage.Source.PathApp, "mednafen.zip"));
        }



        private RelayCommand _recentCommand;

        public RelayCommand RecentCommand
        {
            get
            {
                return _recentCommand ??= new RelayCommand(
                    () => { _navigationService.NavigateTo("Recent"); });
            }
        }

        private RelayCommand _downloadedCommand;

        public RelayCommand DownloadedCommand
        {
            get
            {
                return _downloadedCommand ??= new RelayCommand(
                    () => { _navigationService.NavigateTo("Downloaded"); });
            }
        }


        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

    }
}
