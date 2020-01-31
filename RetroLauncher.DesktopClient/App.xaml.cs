using System;
using System.Windows;
using System.Windows.Threading;

namespace RetroLauncher.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            //DispatcherHelper.Initialize();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Application_ThreadException);
            //DispatcherHelper.UIDispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(Dispatcher_ThreadException);
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

            System.IO.File.WriteAllText("log.txt",exception.ToString()+Environment.NewLine);
        }
    }

    public class ProgressMessage
    {
        public int Percent;
        public string Message;
    }
}
