using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class PanelViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        public PanelViewModel(IRegionManager regionMananger, IDialogService dialogService)
        {
            _regionManager = regionMananger;
            _dialogService = dialogService;
            SettingEmuCommand = new DelegateCommand(ShowDialogSettingEmulator);
            CatalogNavigateCommand = new DelegateCommand(NavigateCatalog);
            RecentNavigateCommand = new DelegateCommand(NavigateRecent);
            DownloadedNavigateCommand = new DelegateCommand(NavigateDownloaded);
        }

        public DelegateCommand SettingEmuCommand { get; private set; }
        public DelegateCommand CatalogNavigateCommand { get; private set; }
        public DelegateCommand RecentNavigateCommand { get; private set; }
        public DelegateCommand DownloadedNavigateCommand { get; private set; }


        private void ShowDialogSettingEmulator()
        {
            _dialogService.ShowDialog("SettingEmulatorDialog", new DialogParameters(), null);
        }

        private void NavigateCatalog()
        {
            _regionManager.RequestNavigate("CatalogRegion", "CatalogView");
        }

        private void NavigateRecent()
        {
            _regionManager.RequestNavigate("CatalogRegion", "RecentView");
        }

        private void NavigateDownloaded()
        {
            _regionManager.RequestNavigate("CatalogRegion", "DownloadedView");
        }
    }
}
