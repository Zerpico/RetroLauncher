using Prism.Mvvm;
using Prism.Regions;
using RetroLauncher.ServiceTools.Download;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace RetroLauncher.Client.ViewModels
{
    public class DownloadListViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DownloadListViewModel(IRegionManager regionMananger)
        {
            _regionManager = regionMananger;
            DownloadsList = DownloadManager.Instance.DownloadsList;//.CollectionChanged += DownloadsList_CollectionChanged;
        }

        private void DownloadsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DownloadList = (sender as ObservableCollection<WebDownloadClient>).Select(d => new DownloadListDTO() { FileName = d.FileName, PercentString = d.PercentString }).ToList();
        }

        public ObservableCollection<WebDownloadClient> DownloadsList { get; set; }

        private List<DownloadListDTO> downloadList;
        public List<DownloadListDTO> DownloadList 
        {
            get => downloadList;
            set => SetProperty(ref downloadList, value);
        }
    }

    public class DownloadListDTO
    {
        public string FileName { get; set; }
        public string PercentString { get; set; }
    }
}
