using RetroLauncher.Client.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace RetroLauncher.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FlatWindow1 : FlatWindow
    {
        bool isMouseButtonDown = false;
        public FlatWindow1()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string[] sources = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory()+@"\icons\");
           
            Random rnd = new Random();
            System.Timers.Timer timer = new System.Timers.Timer(1500);
            timer.Elapsed += new ElapsedEventHandler(delegate (Object o, ElapsedEventArgs a)
            {              
                icon_retro.Dispatcher.BeginInvoke(new Action(() =>
                    icon_retro.Source = new BitmapImage(new Uri(sources[rnd.Next(0, sources.Length - 1)]))));
            });

            timer.Start();
        }



        #region Click events
        private void HeaderBar_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
                isMouseButtonDown = true;
            }
        }
        private void HeaderBar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseButtonDown = false;
        }
        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

    }

}
