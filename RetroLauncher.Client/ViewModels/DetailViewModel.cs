using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using RetroLauncher.Client.Models;
using RetroLauncher.DAL.Service;
using RetroLauncher.ServiceTools.Download;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RetroLauncher.ServiceTools;
using System.Windows.Threading;
using System.Threading.Tasks;
using Prism.Services.Dialogs;

namespace RetroLauncher.Client.ViewModels
{
    public class DetailViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository _repository;
        private readonly IDialogService _dialogService;
        public DetailViewModel(IRegionManager regionMananger, IRepository repository, IDialogService dialogService)
        {
            _regionManager = regionMananger;
            _repository = repository;
            _dialogService = dialogService;
            GoBackCommand = new DelegateCommand(ShowCatalogView);
            DownloadCommand = new DelegateCommand(DownloadGame, CanPlay);
        }


        private void ShowDialog()
        {
            var path = Storage.Source.GetGameFolder(SelectedGame);
            _dialogService.ShowDialog("SelectFileDialog", new DialogParameters($"path={path}"), DialogResultCallback);
        }

        private void DialogResultCallback(IDialogResult obj)
        {
            if (obj.Result == ButtonResult.OK)
            {
                var file = obj.Parameters.GetValue<string>("file");
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Storage.Source.PathEmulatorExe;
                process.StartInfo.Arguments = $"\"{file}\"";
                process.Start();
            }
        }

        public int Percent
        {
            get
            {
                if (DownloadManager.Instance.DownloadsList.Count == 0) return 0;
                var dwlnd = DownloadManager.Instance.DownloadsList.Where(d=>d.Id == SelectedGame.GameId).FirstOrDefault();
                if (dwlnd == null) return 0; else return (int)dwlnd.Progress;
            }
        }
        private GameUI selectedGame;
        public GameUI SelectedGame
        {
            get => selectedGame;
            set => SetProperty(ref selectedGame, value);
        }


        #region Commands
        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand DownloadCommand { get; private set; }

        #endregion

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
            RaisePropertyChanged("Percent");
        }

        bool CanPlay()
        {
            if (DownloadManager.Instance.DownloadsList.Count == 0) return true;
            var dwlnd = DownloadManager.Instance.DownloadsList.Where(d => d.Id == SelectedGame.GameId).FirstOrDefault();
            if (dwlnd.Status == DownloadStatus.Completed) return true;
            else if (dwlnd.Status == DownloadStatus.Initialized) return true;
            else return false;
        }

        void DownloadGame()
        {            
            WebDownloadClient download = new WebDownloadClient(SelectedGame.GameId, SelectedGame.RomUrl);

            download.FileName = Path.Combine(Storage.Source.GetGameFolder(SelectedGame), SelectedGame.RomUrl.Substring(SelectedGame.RomUrl.LastIndexOf("/") + 1));
            

            // Register WebDownloadClient events
            download.DownloadProgressChanged += DownloadProgressChangedHandler;//+= download.DownloadProgressChangedHandler;
            download.DownloadCompleted += DownloadCompletedHandler;
         
            string filePath = download.FileName;
            string tempPath = filePath + ".tmp";

            download.CheckUrl();
            if (download.HasError)
                return;

            download.TempDownloadPath = tempPath;
            download.AddedOn = DateTime.UtcNow;
            download.CompletedOn = DateTime.MinValue;


            // Add the download to the downloads list
            DownloadManager.Instance.DownloadsList.Add(download);
            download.Start();
            

            DownloadCommand.RaiseCanExecuteChanged();
        }

        private void DownloadProgressChangedHandler(object sender, EventArgs e)
        {
            RaisePropertyChanged("Percent");
        }

        private void DownloadCompletedHandler(object sender, EventArgs e)
        {
            RaisePropertyChanged("Percent");
            ArchiveExtractor.ExtractAll(Path.Combine(Storage.Source.GetGameFolder(SelectedGame), SelectedGame.RomUrl.Substring(SelectedGame.RomUrl.LastIndexOf("/") + 1)),
            Storage.Source.GetGameFolder(SelectedGame), true);

            DownloadCommand.RaiseCanExecuteChanged();
            SomeMethod();

            System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
            (Action)delegate ()
            {
                ShowDialog();
            });
        }

        private void SomeMethod()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
            (Action)delegate ()
            {
                DownloadManager.Instance.ClearDownload(SelectedGame.GameId);
            });
           
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }


    }
}
