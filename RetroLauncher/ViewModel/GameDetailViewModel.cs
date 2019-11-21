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
using RetroLauncher.Service;

namespace RetroLauncher.ViewModel
{
    public class GameDetailViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IRepository _repository;
        FileDownloader fileDownloader;

        private IGame selectedGame;
        public IGame SelectedGame { get { return selectedGame; } set { selectedGame = value; } }

        public GameDetailViewModel(IFrameNavigationService navigationService, IRepository repository)
        {
            //инициализируем всякие репы и прочии службы
            _repository = repository;
            _navigationService = navigationService;
            MessengerInstance.Register<Game>(this, RefreshGame);
            //получаем инфу об игре которую выбрали
            RefreshGame((Game)_navigationService.Parameter);

            //Тут использую кортеж так как хотим видеть 2 значения а в контсруктор можно только один. Можно заменить например на клас.
            var progress = new Progress<(int progress, string bytes)>(
             (value) =>
             {
                 Progress = value.progress;
                 DownloadBytes = value.bytes;
             });
            fileDownloader = new FileDownloader(progress);

            //fileDownloader.DownloadComplete += () => { fileDownloader.ProgressChanged -= ProgressChanged; Progress = 0; DownloadBytes="";};
        }

        /// <summary>
        /// обновление информации о игре для просмотра
        /// </summary>
        /// <param name="recGame"></param>
        /// <returns></returns>
        private async void RefreshGame(Game recGame)
        {
            selectedGame =  await _repository.GetGameById(recGame.GameId);
            RaisePropertyChanged(nameof(SelectedGame));
        }

        //навигация назад
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

                    },() => Progress == 0));
            }
        }

        //команда скачать игру
        private RelayCommand _downloadCommand;
        public RelayCommand DownloadCommand
        {
            get
            {
                return _downloadCommand
                    ?? (_downloadCommand = new RelayCommand(() =>
                    {

                        fileDownloader.DownloadFile((Game)SelectedGame);

                    }, () => Progress == 0)); //заблокируем нах кнопку если уже что-то скачиваем
            }
        }

        //прогресс скачивания в процентах
        private double progress;
        public double Progress
        {
            get => this.progress;
            set
            {
                this.progress = value;
                RaisePropertyChanged(nameof(Progress));
                //ужасный костыль (возможно)
                DownloadCommand.RaiseCanExecuteChanged();
                NavigateBackCommand.RaiseCanExecuteChanged();
            }
        }

        //кол-во скачаных байт
        private string downloadBytes;
        public string DownloadBytes
        {
            get => this.downloadBytes;
            set
            {
                this.downloadBytes = value;
                RaisePropertyChanged(nameof(DownloadBytes));
            }
        }
    }
}
