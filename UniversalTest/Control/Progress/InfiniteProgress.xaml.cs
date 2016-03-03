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
        #region field
        private Storyboard _storyboard;
        private bool _isStoped;
        #endregion

        #region ctor
        public InfiniteProgress()
        {
            this.InitializeComponent();

            _storyboard = new Storyboard();
            Loaded += InfiniteProgress_Loaded;
        }
        #endregion

        #region property
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (double), typeof (InfiniteProgress), new PropertyMetadata(20));

        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        #endregion

        #region public
        /// <summary>
        /// 开始循环动画
        /// </summary>
        public void Begin()
        {
            _storyboard.Children.Clear();

            DoubleAnimation doubleAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = 359.9,
                Duration = TimeSpan.FromMilliseconds(4000),
                EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut },
                FillBehavior = FillBehavior.HoldEnd
            };

            _storyboard.Completed += Sb_Completed;
            Storyboard.SetTarget(doubleAnimation, PieSlice);
            Storyboard.SetTargetProperty(doubleAnimation, "SweepAngle");
            _storyboard.Children.Add(doubleAnimation);

            _isStoped = false;
            _storyboard.Begin();
        }

        /// <summary>
        /// 结束，在此次循环动画结束后播放末尾动画，然后结束
        /// </summary>
        public void Stop()
        {
            _isStoped = true;
        }

        #endregion

        #region private
        private void InfiniteProgress_Loaded(object sender, RoutedEventArgs e)
        {
            var vw = this.ActualWidth / 2;
            var vh = this.ActualHeight / 2;
            RotationYCompositeTransform3D.CenterX = vw;
            RotationYCompositeTransform3D.CenterY = vh;
            PieSlice.TopCenter = new Point(vw, 0);
            PieSlice.Radius = vw;


            Begin();
        }
       
        private void Sb_Completed(object sender, object e)
        {
            if (_isStoped) // 停止了
            {
                BeginEndAnimation();
                return;
            }

            var sb = sender as Storyboard;
            var doubleAnimation = (sb.Children[0] as DoubleAnimation);

            var flag = Math.Abs(RotationYCompositeTransform3D.RotationY) < 0.1;
            if (flag) // blue for now
            {
                RotationYCompositeTransform3D.RotationY = 180;
                doubleAnimation.From = 359.99;
                doubleAnimation.To = 0;
            }
            else
            {
                RotationYCompositeTransform3D.RotationY = 0;
                doubleAnimation.From = 0;
                doubleAnimation.To = 359.99;
            }

            sb?.Begin();
        }

        /// <summary>
        /// 末尾的动画
        /// </summary>
        private void BeginEndAnimation()
        {
            _storyboard.Stop();
            PieSlice.SweepAngle = 359.999;
            _storyboard.Children.Clear();

            // 动画使其strokethickness到radius
            DoubleAnimation doubleAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                To = PieSlice.Radius, 
                Duration = TimeSpan.FromMilliseconds(5000),
                //EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(doubleAnimation, PieSlice);
            Storyboard.SetTargetProperty(doubleAnimation, "StrokeThickness");
            _storyboard.Children.Add(doubleAnimation);
            _storyboard.Completed -= Sb_Completed;
            _storyboard.Begin();
        }
        #endregion

    }
}
