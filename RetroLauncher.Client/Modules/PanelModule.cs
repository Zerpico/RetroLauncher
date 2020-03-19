using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.Modules
{
    public class PanelModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate("PanelRegion", "PanelView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {           
            containerRegistry.RegisterForNavigation<Views.PanelView>();
            containerRegistry.RegisterDialog<Views.SettingEmulatorDialog, ViewModels.SettingEmulatorDialogViewModel>();
            containerRegistry.RegisterDialogWindow<Views.Dialogs.FlatDialogWindow>();
        }
    }
}
