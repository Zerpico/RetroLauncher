using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.Modules
{
    public class DownloadModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate("DownloadRegion", "DownloadListView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<IRepository, WebRepository>();
            //containerRegistry.RegisterForNavigation<Views.CatalogView>();
            containerRegistry.RegisterForNavigation<Views.DownloadListView>();
        }


    }
}
