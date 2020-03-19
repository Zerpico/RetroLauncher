using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class DownloadedViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DownloadedViewModel(IRegionManager regionMananger)
        {
            _regionManager = regionMananger;           
        }
    }
}
