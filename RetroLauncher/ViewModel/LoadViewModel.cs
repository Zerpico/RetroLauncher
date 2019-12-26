using RetroLauncher.Data.Model;
using RetroLauncher.Data.Service;
using RetroLauncher.Helpers;
using RetroLauncher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLauncher.ViewModel.Base;
using RetroLauncher.Service;

namespace RetroLauncher.ViewModel
{
    public class LoadViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;

        public LoadViewModel()
        {
            Progress = 0;
            Message = "Загрузка";
            MessengerInstance.Register<(int, string)>(this, RefreshPage);
        }

        private void RefreshPage((int progres, string message) obj)
        {
            Progress = obj.progres;
            Message = obj.message;
            RaisePropertyChanged(nameof(Progress));
            RaisePropertyChanged(nameof(Message));

        }

        public int Progress { get; set; }
        public string Message { get; set; }
    }
}