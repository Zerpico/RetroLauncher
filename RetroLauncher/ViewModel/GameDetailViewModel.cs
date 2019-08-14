using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RetroLauncher.Helpers;
using RetroLauncher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.ViewModel
{
    public class GameDetailViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;

        private object selGame;

        private Game selectedGame;
        public Game SelectedGame { get { return selectedGame; } set { selectedGame = value; } }

        public GameDetailViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            SelectedGame = (Game)_navigationService.Parameter;           
            MessengerInstance.Register<Game>(this, RefreshGame);
        }

        private void RefreshGame(Game recGame)
        {
            SelectedGame = recGame;
            RaisePropertyChanged(nameof(SelectedGame));
        }


        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand
                    ?? (_backCommand = new RelayCommand(
                    () =>
                    {                       
                        _navigationService.NavigateTo("Home");
                    }));
            }
        }
    }
}
