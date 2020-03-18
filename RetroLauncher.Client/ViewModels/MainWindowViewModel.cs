using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using RetroLauncher.ServiceTools;
using RetroLauncher.ServiceTools.Download;

namespace RetroLauncher.Client.ViewModels
{
    public class FlatWindow1ViewModel : BindableBase
    {
        IRegionManager _manager;

        public FlatWindow1ViewModel(IRegionManager manager)
        {
            _manager = manager;
            DownloadEmulator();

        }

        private DelegateCommand<string> navigateCommand;
        public DelegateCommand<string> NavigateCommand
        {
            get
            {
                return navigateCommand ?? (navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));
            }
        }

        private void ExecuteNavigateCommand(string navigatePath)
        {
            if (string.IsNullOrEmpty(navigatePath))
                throw new ArgumentException();

            _manager.RequestNavigate("CatalogRegion", navigatePath);
        }

        void DownloadEmulator()
        {
            if (!Directory.Exists(ServiceTools.Storage.Source.PathEmulator))
            {
                Directory.CreateDirectory(ServiceTools.Storage.Source.PathEmulator);

                WebDownloadClient download = new WebDownloadClient(0, "https://www.zerpico.ru/retrolauncher/mednafen.zip");

                download.FileName = Path.Combine(ServiceTools.Storage.Source.PathEmulator, download.Url.ToString().Substring(download.Url.ToString().LastIndexOf("/") + 1));

                download.DownloadCompleted += ((e,a)=>
                {
                    ArchiveExtractor.ExtractAll(download.FileName, ServiceTools.Storage.Source.PathEmulator, true);

                    System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                    (Action)delegate ()
                    {
                        DownloadManager.Instance.ClearDownload(0);
                    });
                });

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

            }
        }
    }
}
