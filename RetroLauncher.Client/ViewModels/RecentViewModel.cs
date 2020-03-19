using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class RecentViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public RecentViewModel(IRegionManager regionMananger)
        {
            _regionManager = regionMananger;           
        }
    }
}
