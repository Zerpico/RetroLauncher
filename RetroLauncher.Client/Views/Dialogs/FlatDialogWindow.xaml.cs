using Prism.Services.Dialogs;
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
using System.Windows.Shapes;

namespace RetroLauncher.Client.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для FlatDialogWindow.xaml
    /// </summary>
    public partial class FlatDialogWindow : Window, IDialogWindow
    {
        public FlatDialogWindow()
        {
            InitializeComponent();            
        }

        public IDialogResult Result { get; set; }
    }
}
