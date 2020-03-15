using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace RetroLauncher.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IRegionManager _manager;

        public MainWindowViewModel(IRegionManager manager)
        {
            _manager = manager;
            //ServiceTools.Download.DownloadManager.Instance.
        }

        private DelegateCommand<string> navigateCommand;
        public DelegateCommand<string> NavigateCommand
        {
            get
            {
                return navigateCommand ?? (navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));
            }
        }

        private void ExecuteNavigateCommand(string navigatePath)
        {
            if (string.IsNullOrEmpty(navigatePath))
                throw new ArgumentException();

            _manager.RequestNavigate("CatalogRegion", navigatePath);
        }
    }
}
