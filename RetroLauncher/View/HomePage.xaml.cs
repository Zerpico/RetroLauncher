using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RetroLauncher.View
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }


        private void filtrBtn_Click(object sender, RoutedEventArgs e)
        {

            Button image = sender as Button;
            ContextMenu contextMenu = image.ContextMenu;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            contextMenu.PlacementTarget = image;
            contextMenu.VerticalOffset = 5;
            contextMenu.IsOpen = true;
            e.Handled = true;

        }
    }
}
