using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLauncher.Helpers;
using System.Collections.ObjectModel;
using RetroLauncher.Model;
using RetroLauncher.Controls;
using RetroLauncher.ViewModel.Base;
//using Game = RetroLauncher.Model.Game;
using RetroLauncher.Repository;
using RetroLauncher.DAL.Model;

namespace RetroLauncher.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IRepository _gameDb;

        public HomeViewModel(IFrameNavigationService navigationService, IRepository gameDb)
        {
            _navigationService = navigationService;
            _gameDb = gameDb;
            currentPage = 1;
            maxShowGames = 40;
            _navigationService.ShowWaitPage();
            GetGenres();
            GetPlatforms();
            GetGames();
        }

        public int GenreCheckCount { get { return Genres.Where(d => d.IsChecked).Count(); } }
        public bool GenreCheckVisible { get { return GenreCheckCount > 0; } }
        public ObservableCollection<CheckedListItem<Genre>> Genres { get; set; }

        public int PlatformCheckCount { get { return Platforms.Where(d => d.IsChecked).Count(); } }
        public bool PlatformCheckVisible { get { return PlatformCheckCount > 0; } }
        public ObservableCollection<CheckedListItem<Platform>> Platforms { get; set; }
        //коллекция игр
        public ObservableCollection<GameDTO> Games { get; set; }

        //выбранная текущая игра
        private GameDTO selectedGame;
        public GameDTO SelectedGame
        {
            get { return selectedGame; }
            set
            {
                selectedGame = value;
                RaisePropertyChanged("SelectedGame");
                if (selectedGame != null)
                {
                    MessengerInstance.Send(SelectedGame);
                    _navigationService.NavigateTo("GameDetail", SelectedGame);
                }
            }
        }


        //сколько макс игр показывать на странице
        private int maxShowGames;

        //текущая страница, при её смене обновляем список игр
        private int currentPage;
        public int CurrentPage { get => currentPage; set { currentPage = value; GetGames(); RaisePropertyChanged(nameof(CurrentPage)); } }

        //последняя страница
        private int maxPage;
        public int MaxPage { get => maxPage; set => maxPage = value; }

        //значение поиска имени игры по названию
        private string searchText { get; set; }
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                GetGames();
                RaisePropertyChanged(nameof(SearchText));
            }
        }

        async void GetGenres()
        {
            var db = await _gameDb.GetGenres();
            Genres = new ObservableCollection<CheckedListItem<Genre>>();
            foreach (var g in db.OrderBy(d=>d.GenreName))
            {
                Genres.Add(new CheckedListItem<Genre>(g));
            }
        }

        async void GetPlatforms()
        {
            var db = await _gameDb.GetPlatforms();
            Platforms = new ObservableCollection<CheckedListItem<Platform>>();
            foreach (var g in db)
            {
                Platforms.Add(new CheckedListItem<Platform>(g));
            }
        }

      

        /// <summary>
        /// Получить список игр, включая фильтры и номер страниц
        /// </summary>
        async void GetGames()
        {

            //наши фильтры
            int count = maxShowGames;
            int skip = (currentPage-1)*maxShowGames;
            string filterName = null;
            int[] filterGenre;
            int[] filterPlatform;

            if (!string.IsNullOrEmpty(searchText))
                filterName = searchText;

            if (Genres != null && Genres.Any(g => g.IsChecked))
                filterGenre = Genres.Where(g => g.IsChecked).Select(g => g.Item.GenreId).ToArray();
            else filterGenre = null;

            if (Platforms != null && Platforms.Any(p => p.IsChecked))
                filterPlatform = Platforms.Where(p => p.IsChecked).Select(p => p.Item.PlatformId).ToArray();
            else filterPlatform = null;

            //получение списка
            var db = await _gameDb.GetGameFilter(filterName, filterGenre, filterPlatform, count, skip);           
            Games = new ObservableCollection<GameDTO>(db.Items.Select(db => new GameDTO(db)));

            //найдем макс кол-во страниц
            //и учтём остаток от деления, на последней странице может быть даже всего 1 игра
            //или выводить 1 стр. из 1 а не из 0
            MaxPage = (db.Total / maxShowGames) + ((db.Total % maxShowGames) > 0 ? 1 : 0);

            RaisePropertyChanged(nameof(MaxPage));
            RaisePropertyChanged(nameof(Games));
            RaisePropertyChanged(nameof(GenreCheckCount));
            RaisePropertyChanged(nameof(PlatformCheckCount));
            RaisePropertyChanged(nameof(GenreCheckVisible));
            RaisePropertyChanged(nameof(PlatformCheckVisible));
            PrevPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();

            _navigationService.HideWaitPage();
        }

        #region Commands
        //команда перехода к окну детализации игры
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

        //переход на следующую страницу
        private RelayCommand nextPageCommand;
        public RelayCommand NextPageCommand
        {
            get
            {
                return nextPageCommand ?? (nextPageCommand = new RelayCommand(() =>
                {
                    if (CanMoveToNextPage)
                    {
                        CurrentPage++;
                        PrevPageCommand.RaiseCanExecuteChanged();
                        NextPageCommand.RaiseCanExecuteChanged();
                    }
                },
                () => CanMoveToNextPage));
            }
        }

        //блокировка кнопки если текущая страница последняя
        bool CanMoveToNextPage
        {
            get { return currentPage < maxPage; }
        }

        //переход на предыдущую страницу
        private RelayCommand prevPageCommand;
        public RelayCommand PrevPageCommand
        {
            get
            {
                return prevPageCommand ?? (prevPageCommand = new RelayCommand(() =>
                {
                    if (CanMoveToPrevPage)
                    {
                        CurrentPage--;
                        PrevPageCommand.RaiseCanExecuteChanged();
                        NextPageCommand.RaiseCanExecuteChanged();
                    }
                },
                () => CanMoveToPrevPage));
            }
        }

        //блокировка кнопки если текущая страница первая
        bool CanMoveToPrevPage
        {
            get { return currentPage > 1; }
        }


        //команда выбора жанра из списка фильтра
        private RelayCommand _checkGenreCommand;
        public RelayCommand CheckGenreCommand
        {
            get
            {
                return _checkGenreCommand
                    ?? (_checkGenreCommand = new RelayCommand(
                    () =>
                    {
                        GetGames();
                    }));
            }
        }
        #endregion
    }
}
