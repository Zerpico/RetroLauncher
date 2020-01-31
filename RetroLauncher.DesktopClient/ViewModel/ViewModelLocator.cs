using System;
using RetroLauncher.DesktopClient.Helpers;
using RetroLauncher.DesktopClient.Service;

namespace RetroLauncher.DesktopClient.ViewModel
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
            FileService s = new FileService();


            var builder = new ContainerBuilder();

            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<HomeViewModel>().SingleInstance();
            builder.RegisterType<RecentViewModel>();
            builder.RegisterType<GameDetailViewModel>();
            builder.RegisterType<LoadViewModel>();

            var navigation = SetupNavigation();

            DisplayRootRegistry rootRegistry = new DisplayRootRegistry();
            rootRegistry.RegisterWindowType<FileSelectViewModel, View.FileSelectWindow>();
            builder.RegisterInstance<DisplayRootRegistry>(rootRegistry);

            builder.RegisterInstance<IFrameNavigationService>(navigation);
            builder.RegisterType<WebRestRepository>().As<IRepository>().SingleInstance();

            var container = builder.Build();

            // Set the service locator to an AutofacServiceLocator.
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);


            SetupNavigation();
        }

        private static FrameNavigationService SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("Home", new Uri("../View/HomePage.xaml", UriKind.Relative));
            navigationService.Configure("Recent", new Uri("../View/RecentPage.xaml", UriKind.Relative));
            navigationService.Configure("GameDetail", new Uri("../View/GameDetailPage.xaml", UriKind.Relative));
            navigationService.Configure("WaitPage", new Uri("../View/WaitPage.xaml", UriKind.Relative));
            navigationService.Configure("LoadPage", new Uri("../View/LoadPage.xaml", UriKind.Relative));

            return navigationService;
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
        public LoadViewModel LoadViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoadViewModel>();
            }
        }




        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}
