using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLauncher.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using RetroLauncher.Model;

namespace RetroLauncher.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;
        private string _myProperty = "MainPageText";
        public string MainPageText
        {
            get
            {
                return _myProperty;
            }

            set
            {
                if (_myProperty == value)
                {
                    return;
                }

                _myProperty = value;
                RaisePropertyChanged("MainPageText");
            }
        }

        public ObservableCollection<Game> Games { get; set; }

        
        public Game SelectedGame
        {
            get;
            set;
        }

        private RelayCommand _page1Command;
        public RelayCommand Page1Command
        {
            get
            {
                return _page1Command
                    ?? (_page1Command = new RelayCommand(
                    () =>
                    {
                        //SelectedGame = Services.RepositoryBase.GetGameByIdAsync(Games[0].GameId).Result;
                        //_navigationService.NavigateTo("Page1");
                    }));
            }
        }

        private RelayCommand _detailCommand;
        public RelayCommand DetailCommand
        {
            get
            {
                return _detailCommand
                    ?? (_detailCommand = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send<Game>(SelectedGame);
                        _navigationService.NavigateTo("GameDetail", SelectedGame);
                    }));
            }
        }

        public HomeViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            Games = new ObservableCollection<Game>(Services.RepositoryBase.GetGamesAsync().Result.Distinct());
        }
    }
}
