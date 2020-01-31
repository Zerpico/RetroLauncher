using RetroLauncher.DesktopClient.ViewModel.Base;

namespace RetroLauncher.DesktopClient.ViewModel
{
    public class LoadViewModel : ViewModelBase
    {
       // private readonly IFrameNavigationService _navigationService;

        public LoadViewModel()
        {
            Progress = 0; RaisePropertyChanged(nameof(Progress));
            Message = "Загрузка";
           // MessengerInstance.Register<ProgressMessage>(this, (d)=>{ RefreshPage(d);});
            RefreshPage(new ProgressMessage(){Percent = 0, Message = "Загрузка"});

            MessengerInstance.Register<ProgressMessage>(this, RefreshPage);
        }

        private void RefreshPage(ProgressMessage obj)
        {
            Progress = obj.Percent;
            Message = obj.Message;
            RaisePropertyChanged(nameof(Progress));
            RaisePropertyChanged(nameof(Message));

        }

        public int Progress { get; set; }
        public string Message { get; set; }
    }
}