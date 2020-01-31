using System.Windows.Controls;

namespace RetroLauncher.DesktopClient.View
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class LoadPage : Page
    {
        public LoadPage()
        {
            InitializeComponent();
        }

        public void SetProgress(int progress, string message = "")
        {
            prgsLoad.Value = progress;
            txtLoad.Text = message;
        }
    }
}
