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

        public HomeViewModel(IFrameNavigationService navigationService, IGameDbService gameDb)
        {
            _navigationService = navigationService;
            _gameDb = gameDb;
            currentPage = 1;
            maxShowGames = 100;
            GetGames();
        }

        //коллекция игр
        public ObservableCollection<Game> Games { get; set; }
        
        //выбраная игры из списка
        public Game SelectedGame { get; set; }

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

        /// <summary>
        /// Получить список игр, включая фильтры и номер страниц
        /// </summary>
        async void GetGames()
        {            
            //наши фильтры
            FilterGame filter = new FilterGame();
            if (!string.IsNullOrEmpty(searchText))
                filter.Name = searchText;

            //получение списка
            var db = await _gameDb.GetBaseFilter(currentPage * 100, (currentPage - 1) * 100, filter);
            Games = new ObservableCollection<Game>(db.Item2);

            //найдем макс кол-во страниц
            //и учтём остаток от деления, на последней странице может быть даже всего 1 игра
            //или выводить 1 стр. из 1 а не из 0
            MaxPage = (db.Item1 / maxShowGames) + ((db.Item1 % maxShowGames) > 0 ? 1 : 0);

            RaisePropertyChanged(nameof(MaxPage));
            RaisePropertyChanged(nameof(Games));
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
        #endregion
    }
}
