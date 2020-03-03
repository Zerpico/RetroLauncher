using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class DetailViewModel : BindableBase
    {
        public string ViewText { get; set; }

        private readonly IRegionManager _regionManager;

        public DetailViewModel(IRegionManager regionMananger)
        {
            _regionManager = regionMananger;
            ButtonCommand = new DelegateCommand(ShowCatalogView);
            ViewText = "Hello From DetailViewModel";
        }


        #region Commands
        public DelegateCommand ButtonCommand { get; private set; }

        #endregion

        #region Private Methods
        private void ShowCatalogView()
        {
            _regionManager.RequestNavigate("CatalogRegion", "CatalogView");
        }

        #endregion
    }
}
