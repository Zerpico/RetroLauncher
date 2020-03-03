using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class CatalogViewModel : BindableBase
    {
        public string ViewText { get; set; }

        private readonly IRegionManager _regionManager;

        public CatalogViewModel(IRegionManager regionMananger)
        {
            _regionManager = regionMananger;
            ButtonCommand = new DelegateCommand(ShowDetailView);
            ViewText = "Hello From CatalogViewModel";
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
}
