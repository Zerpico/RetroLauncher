using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace RetroLauncher.Controls
{
public class FaderFrame : Frame
{
    #region FadeDuration
    public static readonly DependencyProperty FadeDurationProperty =
        DependencyProperty.Register("FadeDuration", typeof(Duration), typeof(FaderFrame),
            new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(150))));

    public static readonly DependencyProperty SlideDurationProperty =
       DependencyProperty.Register("SlideDuration", typeof(Duration), typeof(FaderFrame),
           new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(200))));
        /// <summary>
        /// FadeDuration will be used as the duration for Fade Out and Fade In animations
        /// </summary>
        public Duration FadeDuration
    {
        get { return (Duration)GetValue(FadeDurationProperty); }
        set { SetValue(FadeDurationProperty, value); }
    }

        public Duration SlideDuration
        {
            get { return (Duration)GetValue(SlideDurationProperty); }
            set { SetValue(SlideDurationProperty, value); }
        }
        #endregion
        public FaderFrame()
        : base()
    {
        // watch for navigations
        Navigating += OnNavigating;
    }
    public override void OnApplyTemplate()
    {
        // get a reference to the frame's content presenter
        // this is the element we will fade in and out
        _contentPresenter = GetTemplateChild("PART_FrameCP") as ContentPresenter;
        base.OnApplyTemplate();
    }
    protected void OnNavigating(object sender, NavigatingCancelEventArgs e)
    {
            // if we did not internally initiate the navigation:
            //   1. cancel the navigation,
            //   2. cache the target,
            //   3. disable hittesting during the fade, and
            //   4. fade out the current content
            if (Content != null && !_allowDirectNavigation && _contentPresenter != null)
            {
                e.Cancel = true;
                _navArgs = e;
                _contentPresenter.IsHitTestVisible = false;

                DoubleAnimation da = new DoubleAnimation(0.0d, FadeDuration);
                ThicknessAnimation animation = new ThicknessAnimation(new Thickness(_contentPresenter.ActualWidth, 0, 0, 0), SlideDuration); // √енерируем анимацию в коде на основе Offset
                    animation.Completed += OnAnimationCompleted; //Ќужно подписатьс€ на окончание анимации

                da.DecelerationRatio = 1.0d;
                da.Completed += FadeOutCompleted;
                _contentPresenter.BeginAnimation(OpacityProperty, da);
                _contentPresenter.BeginAnimation(MarginProperty, animation);
            }
        _allowDirectNavigation = false;

    }

        private void OnAnimationCompleted(object sender, EventArgs e)
        {
            (sender as AnimationClock).Completed -= OnAnimationCompleted;
            //проставить Margin=0 и Grid.Row=1, так как теперь панель должна находитьс€ в другой колонке
            _contentPresenter.Margin = new Thickness();
            
            if (_contentPresenter != null)
            {
                
                
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                    (ThreadStart)delegate ()
                    {
                        ThicknessAnimation da = new ThicknessAnimation(new Thickness(0,0,0,0), SlideDuration);
                        da.AccelerationRatio = 1.0d;
                        _contentPresenter.BeginAnimation(MarginProperty, da);
                    });
            }
        }

        private void FadeOutCompleted(object sender, EventArgs e)
    {
        // after the fade out
        //   1. re-enable hittesting
        //   2. initiate the delayed navigation
        //   3. invoke the FadeIn animation at Loaded priority
        (sender as AnimationClock).Completed -= FadeOutCompleted;
        if (_contentPresenter != null)
        {
            _contentPresenter.IsHitTestVisible = true;
            _allowDirectNavigation = true;
            switch (_navArgs.NavigationMode)
            {
                case NavigationMode.New:
                    if (_navArgs.Uri == null)
                    {
                        NavigationService.Navigate(_navArgs.Content);
                    }
                    else
                    {
                        NavigationService.Navigate(_navArgs.Uri);
                    }
                    break;
                case NavigationMode.Back:
                        NavigationService.Navigate(_navArgs.Uri);
                        break;
                case NavigationMode.Forward:
                    NavigationService.GoForward();
                    break;
                case NavigationMode.Refresh:
                    NavigationService.Refresh();
                    break;
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                (ThreadStart)delegate()
            {
                DoubleAnimation da = new DoubleAnimation(1.0d, FadeDuration);
                da.AccelerationRatio = 1.0d;
                _contentPresenter.BeginAnimation(OpacityProperty, da);                
            });
               
            }
    }
    private bool _allowDirectNavigation = false;
    private ContentPresenter _contentPresenter = null;
    private NavigatingCancelEventArgs _navArgs = null;
}
}