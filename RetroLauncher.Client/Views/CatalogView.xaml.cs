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
            gameList.ScrollIntoView(gameList.Items[0]);
        }

     
    }
}
