using GalaSoft.MvvmLight.Views;

namespace RetroLauncher.Helpers
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
