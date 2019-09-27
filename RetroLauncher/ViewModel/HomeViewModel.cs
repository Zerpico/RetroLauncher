using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLauncher.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using RetroLauncher.Data.Model;
using RetroLauncher.Data.Service;
using RetroLauncher.Model;
using RetroLauncher.Controls;

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
            filter = new FilterGame();
            maxShowGames = 50;
            GetGames();
            GetGenres();
            GetPlatforms();
        }

        public ObservableCollection<CheckedListItem<string>> Genres { get; set; }
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
            Genres = new ObservableCollection<CheckedListItem<string>>();
            foreach (var g in db.OrderBy(d=>d))
            {
                Genres.Add(new CheckedListItem<string>(g));
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

            //получение списка
            var db = await _gameDb.GetBaseFilter(filter);
            Games = new ObservableCollection<IGame>(db.Item2);

            //найдем макс кол-во страниц
            //и учтём остаток от деления, на последней странице может быть даже всего 1 игра
            //или выводить 1 стр. из 1 а не из 0
            MaxPage = (db.Item1 / maxShowGames) + ((db.Item1 % maxShowGames) > 0 ? 1 : 0);

            RaisePropertyChanged(nameof(MaxPage));
            RaisePropertyChanged(nameof(Games));
            PrevPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();

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
                        var ff = Genres.Where(d=>d.IsChecked).ToList();
                    }));
            }
        }
        #endregion
    }
}
