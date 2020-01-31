using System.Windows;
using System.Windows.Input;

namespace RetroLauncher.DesktopClient.View
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
