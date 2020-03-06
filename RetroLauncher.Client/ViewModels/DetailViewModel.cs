using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using RetroLauncher.Client.Models;
using RetroLauncher.DAL.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class DetailViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository _repository;

        public DetailViewModel(IRegionManager regionMananger, IRepository repository)
        {
            _regionManager = regionMananger;
            _repository = repository;
            GoBackCommand = new DelegateCommand(ShowCatalogView);
        }

        private GameUI selectedGame;
        public GameUI SelectedGame
        {
            get => selectedGame;
            set => SetProperty(ref selectedGame, value);
        }


        #region Commands
        public DelegateCommand GoBackCommand { get; private set; }

        #endregion

        #region Private Methods
        private void ShowCatalogView()
        {
            _regionManager.RequestNavigate("CatalogRegion", "CatalogView");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //get a single parameter as type object, which must be cast

            int gameid = navigationContext.Parameters.GetValue<int>("GameID");
            RefreshGame(gameid);
        }

        async void RefreshGame(int gameid)
        {
            SelectedGame = new GameUI(await _repository.GetGameById(gameid));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
