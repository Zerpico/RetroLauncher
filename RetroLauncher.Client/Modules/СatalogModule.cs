using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using RetroLauncher.WebApi.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.Modules
{
    public class СatalogModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate("CatalogRegion", "CatalogView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IRepository, WebRepository>();
            containerRegistry.RegisterForNavigation<Views.CatalogView>();
            containerRegistry.RegisterForNavigation<Views.DetailView>();
            containerRegistry.RegisterForNavigation<Views.RecentView>();
            containerRegistry.RegisterForNavigation<Views.DownloadedView>();
            containerRegistry.RegisterDialog<Views.SelectFileDialog, ViewModels.SelectFileDialogViewModel>();
            containerRegistry.RegisterDialogWindow<Views.FlatDialogWindow>();
        }


    }
}
