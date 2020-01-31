using System.Windows;
using System.Windows.Controls;

namespace RetroLauncher.DesktopClient.View
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
