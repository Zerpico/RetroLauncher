using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLauncher.Helpers;
using System.Collections.ObjectModel;
using RetroLauncher.ViewModel.Base;
using RetroLauncher.Repository;

namespace RetroLauncher.ViewModel
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