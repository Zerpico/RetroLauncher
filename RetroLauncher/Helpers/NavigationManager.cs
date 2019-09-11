using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace RetroLauncher.Helpers
{
    public class NavigationManager : INavigationManager
    {
        #region Fields

        private readonly Dispatcher _dispatcher;

        private ContentControl frameControl { get; set; }
        private ContentControl _frameControl { get { return frameControl = frameControl ?? GetDescendantFromName(Application.Current.MainWindow, "MainFrame") as ContentControl; } }
        private readonly IDictionary<string, object> _viewModelsByNavigationKey = new Dictionary<string, object>();
        private readonly IDictionary<Type, Type> _viewTypesByViewModelType = new Dictionary<Type, Type>();

        #endregion

        #region Ctor

        public NavigationManager(Dispatcher dispatcher)
        {
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

           /* _frameControl = GetDescendantFromName(Application.Current.MainWindow, "MainFrame") as ContentControl;
             if (_frameControl == null)
                 throw new ArgumentNullException("frameControl");*/
 
            _dispatcher = dispatcher;
           // _frameControl = frameControl;
        }

        #endregion

        public void Register<TViewModel, TView>(TViewModel viewModel, string navigationKey)
            where TViewModel : ViewModelBase
            where TView : FrameworkElement
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");
            if (navigationKey == null)
                throw new ArgumentNullException("navigationKey");

            _viewModelsByNavigationKey[navigationKey] = viewModel;
            _viewTypesByViewModelType[typeof(TViewModel)] = typeof(TView);
        }

        public void Navigate(string navigationKey, object arg = null)
        {
            if (navigationKey == null)
                throw new ArgumentNullException("navigationKey");

            InvokeInMainThread(() =>
            {
                InvokeNavigatingFrom();
                
                var viewModel = GetNewViewModel(navigationKey);
                InvokeNavigatingTo(viewModel, arg);

                var view = CreateNewView(viewModel);
                _frameControl.Content = view;
            });
        }

        private void InvokeInMainThread(Action action)
        {
            _dispatcher.Invoke(action);
        }

        private FrameworkElement CreateNewView(object viewModel)
        {
            var viewType = _viewTypesByViewModelType[viewModel.GetType()];
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            return view;
        }

        private object GetNewViewModel(string navigationKey)
        {
            return _viewModelsByNavigationKey[navigationKey];
        }

        private void InvokeNavigatingFrom()
        {
            var oldView = _frameControl.Content as FrameworkElement;
            if (oldView == null)
                return;

            var navigationAware = oldView.DataContext as INavigationAware;
            if (navigationAware == null)
                return;

            navigationAware.OnNavigatingFrom();
        }

        private static void InvokeNavigatingTo(object viewModel, object arg)
        {
            var navigationAware = viewModel as INavigationAware;
            if (navigationAware == null)
                return;

            navigationAware.OnNavigatingTo(arg);
        }


        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                return null;
            }

            for (var i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }
            return null;
        }
    }
}
