using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RetroLauncher.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
           AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Application_ThreadException);
        }

        private static void Dispatcher_ThreadException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            MessageBox.Show(e.Exception.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error );
        }

        private static void Application_ThreadException(object sender, UnhandledExceptionEventArgs e)
        {
            GenereicExepction(e.ExceptionObject as Exception);
        }

        private static void GenereicExepction(Exception exception)
        {

            System.IO.File.AppendAllText("log.txt","["+DateTime.Now.ToShortDateString()+"]  " +exception.ToString()+Environment.NewLine);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstapper = new Bootstrapper();
            bootstapper.Run();
        }
    }
}
