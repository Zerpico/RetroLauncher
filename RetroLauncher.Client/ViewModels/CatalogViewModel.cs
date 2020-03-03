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
    public class CatalogViewModel : BindableBase
    {
        public string ViewText { get; set; }

        private readonly IRegionManager _regionManager;
        private readonly IRepository _repository;

        public CatalogViewModel(IRegionManager regionMananger, IRepository repository)
        {
            _regionManager = regionMananger;
            _repository = repository;
            ButtonCommand = new DelegateCommand(ShowDetailView);
            ViewText = "Hello From CatalogViewModel";

          //  GetGenres();
          //  GetPlatforms();
            GetGames();
        }


        public int GenreCheckCount { get { return Genres.Where(d => d.IsChecked).Count(); } }
        public bool GenreCheckVisible { get { return GenreCheckCount > 0; } }

        ObservableCollection<CheckedListItem<Genre>> genres;
        public ObservableCollection<CheckedListItem<Genre>> Genres
        {
            get => genres;
            set => SetProperty(ref genres, value);
        }

        ObservableCollection<PlatformUI> platforms;
        public ObservableCollection<PlatformUI> Platforms
        {
            get => platforms;
            set => SetProperty(ref platforms, value);
        }

        ObservableCollection<GameUI> games;
        public ObservableCollection<GameUI> Games
        {
            get => games;
            set => SetProperty(ref games, value);
        }

        //значение поиска имени игры по названию
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                SetProperty(ref searchText, value);
                GetGames();
            }
        }

        async void GetGenres()
        {
            var db = await _repository.GetGenres();
            Genres = new ObservableCollection<CheckedListItem<Genre>>();
            foreach (var g in db.OrderBy(d=>d.GenreName))
            {
                Genres.Add(new CheckedListItem<Genre>(g));
            }
        }

        async void GetPlatforms()
        {

            var db = await _repository.GetPlatforms();
            Platforms = new ObservableCollection<PlatformUI>(db.Select(db => new PlatformUI(db)));
        }

        async void GetGames()
        {
            var db = await _repository.GetGameFilter(searchText, null, null, 50,0);
            Games = new ObservableCollection<GameUI>(db.Items.Select(d => new GameUI(d)));  
        }



        #region Commands
        public DelegateCommand ButtonCommand { get; private set; }

        #endregion

        #region Private Methods
        private void ShowDetailView()
        {
            var gr = _regionManager.Regions;
            _regionManager.RequestNavigate("CatalogRegion", "DetailView");
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
