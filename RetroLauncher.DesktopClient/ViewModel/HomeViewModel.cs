using System.Collections.ObjectModel;
using System.Linq;
using RetroLauncher.DesktopClient.Controls;
using RetroLauncher.DesktopClient.Helpers;
using RetroLauncher.DesktopClient.ViewModel.Base;
using Game = RetroLauncher.DAL.Model.Game;

namespace RetroLauncher.DesktopClient.ViewModel
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
            filter = new FilterGame();
            maxShowGames = 50;
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
        public ObservableCollection<IGame> Games { get; set; }

        //выбранная текущая игра
        private Game selectedGame;
        public Game SelectedGame
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

        FilterGame filter { get; set; }

        /// <summary>
        /// Получить список игр, включая фильтры и номер страниц
        /// </summary>
        async void GetGames()
        {

            //наши фильтры
            filter.Count = 100;
            filter.Skip = (currentPage-1)*100;

            if (!string.IsNullOrEmpty(searchText))
                filter.Name = searchText;

            if (Genres != null && Genres.Any(g => g.IsChecked))
                filter.Genre = Genres.Where(g => g.IsChecked).Select(g => g.Item.GenreId).ToArray();
            else filter.Genre = null;

            if (Platforms != null && Platforms.Any(p => p.IsChecked))
                filter.Platform = Platforms.Where(p => p.IsChecked).Select(p => p.Item.PlatformId).ToArray();
            else filter.Platform = null;

            //получение списка
            var db = await _gameDb.GetBaseFilter(filter);
            Games = new ObservableCollection<IGame>(db.Item2);

            //найдем макс кол-во страниц
            //и учтём остаток от деления, на последней странице может быть даже всего 1 игра
            //или выводить 1 стр. из 1 а не из 0
            MaxPage = (db.Item1 / maxShowGames) + ((db.Item1 % maxShowGames) > 0 ? 1 : 0);

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
