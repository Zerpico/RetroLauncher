using GalaSoft.MvvmLight;
using RetroLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.ViewModel
{
    public class RecentViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;

        public RecentViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
