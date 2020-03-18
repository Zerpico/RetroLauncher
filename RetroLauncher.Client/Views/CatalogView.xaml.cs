using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RetroLauncher.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для CatalogView.xaml
    /// </summary>
    public partial class CatalogView : UserControl
    {
        public CatalogView()
        {
            InitializeComponent();
            gameList.Items.CurrentChanged += Items_CurrentChanged;
        }

        private void Items_CurrentChanged(object sender, EventArgs e)
        {
            if (gameList.Items.Count > 0)
                gameList.ScrollIntoView(gameList.Items[0]);
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
