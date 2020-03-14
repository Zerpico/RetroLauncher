using Prism.Modularity;
using Prism.Unity;
using RetroLauncher.Client.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace RetroLauncher.Client
{
    [Obsolete]
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<Views.FlatWindow1>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
           
        }


        protected override void ConfigureModuleCatalog()
        {           
            ModuleCatalog.AddModule<СatalogModule>();
            ModuleCatalog.AddModule<DownloadModule>();
        }

       
    }
}
