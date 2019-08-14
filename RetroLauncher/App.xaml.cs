using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using RetroLauncher.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace RetroLauncher
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            RegisterServices();

            DispatcherHelper.Initialize();
        }

        /// <summary>
        /// Зарегестрировать наши сервисы в DI
        /// </summary>
        private static void RegisterServices()
        {
            SimpleIoc.Default.Register<IFileUrlService, YadiskFileUrlService>(true);
            SimpleIoc.Default.Register<IGameDbService, SqliteGameDbService>(true);
        }
    }
}
