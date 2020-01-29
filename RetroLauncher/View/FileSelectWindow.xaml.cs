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
using System.Windows.Shapes;

namespace RetroLauncher.View
{
    public partial class FileSelectWindow : Window
    {
        public FileSelectWindow()
        {
            InitializeComponent();
        }

        private void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listBox_games.SelectedItem != null)
            {
                 this.DialogResult = true;
            }
        }

    }
}
