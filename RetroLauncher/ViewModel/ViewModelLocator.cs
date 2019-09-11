using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using RetroLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLauncher.Data.Service;
using RetroLauncher.Service;

namespace RetroLauncher.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RecentViewModel>();
          //  SimpleIoc.Default.Register<DownloadedViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<GameDetailViewModel>();
            SetupNavigation();
        }

        private static void SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("Home", new Uri("../View/HomePage.xaml", UriKind.Relative));
            navigationService.Configure("Recent", new Uri("../View/RecentPage.xaml", UriKind.Relative));
          /*  navigationService.Configure("Downloaded", new Uri("../View/DownloadedPage.xaml", UriKind.Relative));*/
            navigationService.Configure("GameDetail", new Uri("../View/GameDetailPage.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
            SimpleIoc.Default.Register<IRepository, WebRestRepository>(true);
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public HomeViewModel HomeViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }

        public RecentViewModel RecentViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RecentViewModel>();
            }
        }

        public GameDetailViewModel GameDetailViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GameDetailViewModel>();
            }
        }
        /*public DownloadedViewModel DownloadedViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DownloadedViewModel>();
            }
        }


        */

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}
