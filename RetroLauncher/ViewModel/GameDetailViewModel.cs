using RetroLauncher.Helpers;
using RetroLauncher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLauncher.ViewModel.Base;
using RetroLauncher.Service;
using RetroLauncher.Repository;

namespace RetroLauncher.ViewModel
{
    public class GameDetailViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IRepository _repository;
        FileDownloader fileDownloader;

        private GameDTO selectedGame;
        public GameDTO SelectedGame { get { return selectedGame; } set { selectedGame = value; } }

        string downloadPath;

        public GameDetailViewModel(IFrameNavigationService navigationService, IRepository repository)
        {
            //инициализируем всякие репы и прочии службы
            _repository = repository;
            _navigationService = navigationService;
            MessengerInstance.Register<GameDTO>(this, RefreshGame);
            //получаем инфу об игре которую выбрали
            RefreshGame((GameDTO)_navigationService.Parameter);

            //Тут использую кортеж так как хотим видеть 2 значения а в контсруктор можно только один. Можно заменить например на клас.
            var progress = new Progress<(int progress, string bytes)>(
             (value) =>
             {
                 Progress = value.progress;
                 DownloadBytes = value.bytes;

             });
            fileDownloader = new FileDownloader(progress);
            fileDownloader.DownloadComplete += downComplete;

            //fileDownloader.DownloadComplete += () => { fileDownloader.ProgressChanged -= ProgressChanged; Progress = 0; DownloadBytes="";};
        }

        private void downComplete()
        {
            Service.ArchiveExtractor.ExtractAll(downloadPath, System.IO.Path.GetDirectoryName(downloadPath));
            System.Threading.Thread.Sleep(10);
            System.IO.File.Delete(downloadPath);
            RunOrDownloadGame();
        }

        /// <summary>
        /// обновление информации о игре для просмотра
        /// </summary>
        /// <param name="recGame"></param>
        /// <returns></returns>
        private async void RefreshGame(GameDTO recGame)
        {
            selectedGame = new GameDTO(await _repository.GetGameById(recGame.GameId));
            FileService serv = new FileService();
           // selectedGame = await serv.GetGame(selectedGame); //todo: переделать этот бред

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


                        RunOrDownloadGame();


                    }, () => Progress == 0 || SelectedGame != null)); //заблокируем нах кнопку если уже что-то скачиваем
            }
        }

        private async void RunOrDownloadGame()
        {
            if (SelectedGame.IsInstall)
            {
                var rootRegistry = (DisplayRootRegistry)CommonServiceLocator.ServiceLocator.Current.GetInstance(typeof(DisplayRootRegistry));

                var fileVM = new FileSelectViewModel(SelectedGame.LocalPathRom);
                var result = await rootRegistry.ShowModalPresentation(fileVM);

                if (result == true)
                {
                    Service.EmulatorService emulator = new EmulatorService();
                    emulator.StartRom(System.IO.Path.Combine(SelectedGame.LocalPathRom, fileVM.SelectRom));
                }

                //var f_rom = System.IO.Directory.GetFiles(SelectedGame.LocalPathRom)[0];

            }
            else
            {
                downloadPath = fileDownloader.DownloadGame(SelectedGame);
                FileService serv = new FileService();

                SelectedGame.LocalPath = new GamePath() { GameId = SelectedGame.GameId, LocalPath = System.IO.Path.GetDirectoryName(downloadPath) };
               // serv.UpdateGame(SelectedGame); //todo: переделать
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
