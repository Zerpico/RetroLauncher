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
using RetroLauncher.Services;

namespace RetroLauncher.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IGameDbService _gameDb;

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
                RaisePropertyChanged(nameof(MainPageText));
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
                        MessengerInstance.Send(SelectedGame);
                        _navigationService.NavigateTo("GameDetail", SelectedGame);
                    }));
            }
        }

        public HomeViewModel(IFrameNavigationService navigationService, IGameDbService gameDb)
        {
            _navigationService = navigationService;
            _gameDb = gameDb;

            Games = new ObservableCollection<Game>(_gameDb.GetGamesAsync().Result.Distinct());
        }
    }
}
