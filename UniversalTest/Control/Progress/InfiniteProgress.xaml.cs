using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Progress
{
    public sealed partial class InfiniteProgress : UserControl
    {
        public InfiniteProgress()
        {
            this.InitializeComponent();
            Loaded += InfiniteProgress_Loaded;

        }

        private void InfiniteProgress_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = 359.9,
                Duration = TimeSpan.FromMilliseconds(3000),
                EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard sb = new Storyboard();
            sb.Completed += Sb_Completed;
            Storyboard.SetTarget(doubleAnimation, PieSlice);
            Storyboard.SetTargetProperty(doubleAnimation, "SweepAngle");
            sb.Children.Add(doubleAnimation);
            sb.Begin();
        }

        private void Sb_Completed(object sender, object e)
        {
            var sb = sender as Storyboard;
            var doubleAnimation = (sb.Children[0] as DoubleAnimation);

            var flag = Math.Abs(RotationYCompositeTransform3D.RotationY) < 0.1;
            if (flag) // blue for now
            {
                RotationYCompositeTransform3D.RotationY = 180;
                //PieSlice.Stroke = WhiteBrush;
                doubleAnimation.From = 359.9;
                doubleAnimation.To = 0;
            }
            else
            {
                RotationYCompositeTransform3D.RotationY = 0;
                //PieSlice.Stroke = BlueBrush;
                doubleAnimation.From = 0;
                doubleAnimation.To = 359.9;
            }

            sb?.Begin();
        }
    }
}
