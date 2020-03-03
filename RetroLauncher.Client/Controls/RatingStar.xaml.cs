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

namespace RetroLauncher.Client.Controls
{
    /// <summary>
    /// Логика взаимодействия для RatingStar.xaml
    /// </summary>
    public partial class RatingStar : UserControl
    {
        public RatingStar()
        {
            InitializeComponent();
        }

        static Brush FillBrush { get; set; }

        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // рейтинг
        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(int),
            typeof(RatingStar),
            new UIPropertyMetadata(0, new PropertyChangedCallback(RatingChanged)));

        // заливка
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush),
            typeof(RatingStar),
            new UIPropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(FillChanged)));

        private static void FillChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            RatingStar s = (RatingStar)depObj;
            FillBrush = (Brush)args.NewValue;
            s.StarOne.Stroke = FillBrush;
            s.StarTwo.Stroke = FillBrush;
            s.StarThree.Stroke = FillBrush;
            s.StarFour.Stroke = FillBrush;
            s.StarFive.Stroke = FillBrush;
        }

        private static void RatingChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            RatingStar s = (RatingStar)depObj;
            int rating = (int)args.NewValue;

            if (rating == 0)
            {
                s.StarOne.Fill = Brushes.Transparent;
                s.StarTwo.Fill = Brushes.Transparent;
                s.StarThree.Fill = Brushes.Transparent;
                s.StarFour.Fill = Brushes.Transparent;
                s.StarFive.Fill = Brushes.Transparent;
            }
            else if (rating == 1)
            {
                s.StarOne.Fill = FillBrush;
                s.StarTwo.Fill = Brushes.Transparent;
                s.StarThree.Fill = Brushes.Transparent;
                s.StarFour.Fill = Brushes.Transparent;
                s.StarFive.Fill = Brushes.Transparent;
            }
            else if (rating == 2)
            {
                s.StarOne.Fill = FillBrush;
                s.StarTwo.Fill = FillBrush;
                s.StarThree.Fill = Brushes.Transparent;
                s.StarFour.Fill = Brushes.Transparent;
                s.StarFive.Fill = Brushes.Transparent;
            }
            else if (rating == 3)
            {
                s.StarOne.Fill = FillBrush;
                s.StarTwo.Fill = FillBrush;
                s.StarThree.Fill = FillBrush;
                s.StarFour.Fill = Brushes.Transparent;
                s.StarFive.Fill = Brushes.Transparent;
            }
            else if (rating == 4)
            {
                s.StarOne.Fill = FillBrush;
                s.StarTwo.Fill = FillBrush;
                s.StarThree.Fill = FillBrush;
                s.StarFour.Fill = FillBrush;
                s.StarFive.Fill = Brushes.Transparent;
            }
            else if (rating == 5)
            {
                s.StarOne.Fill = FillBrush;
                s.StarTwo.Fill = FillBrush;
                s.StarThree.Fill = FillBrush;
                s.StarFour.Fill = FillBrush;
                s.StarFive.Fill = FillBrush;
            }

            //s.txbl_CameraName.Text = args.NewValue.ToString();
        }

        void SetRating(int rating)
        {
            if (rating == 0)
            {
                StarOne.Fill = Brushes.Transparent;
                StarTwo.Fill = Brushes.Transparent;
                StarThree.Fill = Brushes.Transparent;
                StarFour.Fill = Brushes.Transparent;
                StarFive.Fill = Brushes.Transparent;
            }
            else if (rating == 1)
            {
                StarOne.Fill = FillBrush;
               /* StarTwo.Fill = Brushes.Transparent;
                StarThree.Fill = Brushes.Transparent;
                StarFour.Fill = Brushes.Transparent;
                StarFive.Fill = Brushes.Transparent;*/
            }
            else if (rating == 2)
            {
                StarOne.Fill = FillBrush;
                StarTwo.Fill = FillBrush;
                /*StarThree.Fill = Brushes.Transparent;
                StarFour.Fill = Brushes.Transparent;
                StarFive.Fill = Brushes.Transparent;*/
            }
            else if (rating == 3)
            {
                StarOne.Fill = FillBrush;
                StarTwo.Fill = FillBrush;
                StarThree.Fill = FillBrush;
                /*StarFour.Fill = Brushes.Transparent;
                StarFive.Fill = Brushes.Transparent;*/
            }
            else if (rating == 4)
            {
                StarOne.Fill = FillBrush;
                StarTwo.Fill = FillBrush;
                StarThree.Fill = FillBrush;
                StarFour.Fill = FillBrush;
               // StarFive.Fill = Brushes.Transparent;
            }
            else if (rating == 5)
            {
                StarOne.Fill = FillBrush;
                StarTwo.Fill = FillBrush;
                StarThree.Fill = FillBrush;
                StarFour.Fill = FillBrush;
                StarFive.Fill = FillBrush;
            }
        }

        private void StarOne_MouseEnter(object sender, MouseEventArgs e)
        {
            SetRating(1);
        }

        private void StarTwo_MouseEnter(object sender, MouseEventArgs e)
        {
            SetRating(2);
        }

        private void StarThree_MouseEnter(object sender, MouseEventArgs e)
        {
            SetRating(3);
        }

        private void StarFour_MouseEnter(object sender, MouseEventArgs e)
        {
            SetRating(4);
        }

        private void StarFive_MouseEnter(object sender, MouseEventArgs e)
        {
            SetRating(5);
        }





        private void Star_MouseLeave(object sender, MouseEventArgs e)
        {
            //SetRating(0);
        }






        private void StarOne_MouseLeave(object sender, MouseEventArgs e)
        {
            //SetRating(1);
            StarOne.Fill = Brushes.Transparent;
        }

        private void StarTwo_MouseLeave(object sender, MouseEventArgs e)
        {
            //SetRating(2);
            StarOne.Fill = Brushes.Transparent;
            StarTwo.Fill = Brushes.Transparent;
        }

        private void StarThree_MouseLeave(object sender, MouseEventArgs e)
        {
            //SetRating(3);
            StarOne.Fill = Brushes.Transparent;
            StarTwo.Fill = Brushes.Transparent;
            StarThree.Fill = Brushes.Transparent;
        }

        private void StarFour_MouseLeave(object sender, MouseEventArgs e)
        {
            //SetRating(4);
            StarOne.Fill = Brushes.Transparent;
            StarTwo.Fill = Brushes.Transparent;
            StarThree.Fill = Brushes.Transparent;
            StarFour.Fill = Brushes.Transparent;

        }

        private void StarFive_MouseLeave(object sender, MouseEventArgs e)
        {
            //SetRating(5);
            StarOne.Fill = Brushes.Transparent;
            StarTwo.Fill = Brushes.Transparent;
            StarThree.Fill = Brushes.Transparent;
            StarFour.Fill = Brushes.Transparent;
            StarFive.Fill = Brushes.Transparent;
        }
    }
}
