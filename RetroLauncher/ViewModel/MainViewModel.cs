using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using RetroLauncher.Helpers;

namespace RetroLauncher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;
        private RelayCommand _loadedCommand;
        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Home");
                        RaisePropertyChanged("LoadedCommand");
                    }));
            }
        }

        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;            
        }

        public string PropText { get { return "sample"; } }
    }
}
