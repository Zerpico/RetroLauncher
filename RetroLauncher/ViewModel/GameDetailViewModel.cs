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
using Game = RetroLauncher.Model.Game;

namespace RetroLauncher.ViewModel
{
    public class GameDetailViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IRepository _repository;

        private IGame selectedGame;
        public IGame SelectedGame { get { return selectedGame; } set { selectedGame = value; } }

        public GameDetailViewModel(IFrameNavigationService navigationService, IRepository repository)
        {
            _repository = repository;
            _navigationService = navigationService;
            MessengerInstance.Register<Game>(this, RefreshGame);
            RefreshGame((Game)_navigationService.Parameter);
        }

        private async void RefreshGame(Game recGame)
        {
            selectedGame =  await _repository.GetGameById(recGame.GameId);
            RaisePropertyChanged(nameof(SelectedGame));
        }

        private RelayCommand _navigateBackCommand;
        public RelayCommand NavigateBackCommand
        {
            get
            {
                return _navigateBackCommand
                    ?? (_navigateBackCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.GoBack();

                    }));
            }
        }

        private RelayCommand _downloadCommand;
        public RelayCommand DownloadCommand
        {
            get
            {
                return _downloadCommand
                    ?? (_downloadCommand = new RelayCommand(
                    () =>
                    {
                        //bla-bla-bla

                    }));
            }
        }
    }
}
