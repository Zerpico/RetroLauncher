using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using RetroLauncher.Client.Models;
using RetroLauncher.DAL.Model;
using RetroLauncher.DAL.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class CatalogViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository _repository;

        public CatalogViewModel(IRegionManager regionMananger, IRepository repository)
        {
            _regionManager = regionMananger;
            _repository = repository;
            GameSelectCommand = new DelegateCommand(SelectGame);
            NextPageCommand = new DelegateCommand(NextPage);
            PrevPageCommand = new DelegateCommand(PrevPage);
            CurrentPage = 1;
            maxListShow=50;

            GetGenres();
            GetPlatforms();
            GetGames();
        }

        int maxListShow;

        #region Properties

        public int GenreCheckCount { get { return Genres.Where(d => d.IsChecked).Count(); } }
        public bool GenreCheckVisible { get { return GenreCheckCount > 0; } }

        private ObservableCollection<CheckedListItem<Genre>> genres;
        public ObservableCollection<CheckedListItem<Genre>> Genres
        {
            get => genres;
            set => SetProperty(ref genres, value);
        }

        private ObservableCollection<PlatformUI> platforms;
        public ObservableCollection<PlatformUI> Platforms
        {
            get => platforms;
            set => SetProperty(ref platforms, value);
        }

        private ObservableCollection<GameUI> games;
        public ObservableCollection<GameUI> Games
        {
            get => games;
            set => SetProperty(ref games, value);
        }

        private GameUI selectedGame;
        public GameUI SelectedGame
        {
            get => selectedGame;
            set { SetProperty(ref selectedGame, value); if (selectedGame != null) SelectGame(); }
        }

        //значение поиска имени игры по названию
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                SetProperty(ref searchText, value);
                GetGames(true);
            }
        }

        private int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }

        private int maxPage;
        public int MaxPage
        {
            get => maxPage;
            set => SetProperty(ref maxPage, value);
        }

        #endregion

        #region Commands
        public DelegateCommand GameSelectCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }
        public DelegateCommand PrevPageCommand { get; private set; }

        #endregion

        #region Private Methods
        private void SelectGame()
        {

            var query = new NavigationParameters();
            query.Add("GameID", SelectedGame.GameId);
            _regionManager.RequestNavigate("CatalogRegion",
                new Uri("DetailView" + query.ToString(), UriKind.Relative));

        }

        async void GetGenres()
        {
            var db = await _repository.GetGenres();
            if (db == null) return;
            Genres = new ObservableCollection<CheckedListItem<Genre>>();
            foreach (var g in db.OrderBy(d=>d.GenreName))
            {
                Genres.Add(new CheckedListItem<Genre>(g));
            }
        }

        async void GetPlatforms()
        {
            var db = await _repository.GetPlatforms();
            if (db == null) return;
            Platforms = new ObservableCollection<PlatformUI>(db.Select(db => new PlatformUI(db)));
        }

        async void GetGames(bool resetPages = false)
        {
            int skip = resetPages ? 0 : (currentPage-1) * maxListShow;
            var db = await _repository.GetGameFilter(searchText, null, null, maxListShow,skip);
            if (db == null) { Games = new ObservableCollection<GameUI>(); return;}
            Games = new ObservableCollection<GameUI>(db.Items.Select(d => new GameUI(d)));
            MaxPage = (db.Total / maxListShow) + ((db.Total % maxListShow) > 0 ? 1 : 0);
            if (resetPages) CurrentPage = 1;
        }

        private void NextPage()
        {
            CurrentPage++;
            GetGames();
        }

        private void PrevPage()
        {

            CurrentPage--;
            GetGames();

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedGame = null;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;//throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }



    public class CheckedListItem<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isChecked;
        private T item;

        public CheckedListItem()
        { }

        public CheckedListItem(T item, bool isChecked = false)
        {
            this.item = item;
            this.isChecked = isChecked;
        }

        public T Item
        {
            get { return item; }
            set
            {
                item = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Item"));
            }
        }


        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
    }
}
