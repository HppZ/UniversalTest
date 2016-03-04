using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sight.Windows10.Views.Album.Componets.PieControl
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
            _storyboard.Completed += Sb_Completed;
            Loaded += InfiniteProgress_Loaded;
        }
        #endregion

        public Action FinishedAction { get; set; }

        #region property
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(InfiniteProgress), new PropertyMetadata(20));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        #endregion

        #region public
        /// <summary>
        /// 开始循环动画
        /// </summary>
        public void Begin()
        {
            _storyboard.Stop();
            _storyboard.Children.Clear();
            Init();

            DoubleAnimation doubleAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = 359.9,
                Duration = TimeSpan.FromMilliseconds(2000),
                EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut,Exponent = 7},
                FillBehavior = FillBehavior.HoldEnd
            };

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

        public void Cancel()
        {
            
        }

        #endregion

        #region private
        private void InfiniteProgress_Loaded(object sender, RoutedEventArgs e)
        {
            Init2();
        }

        private void ScaleStoryboard_OnCompleted(object sender, object e)
        {
            FinishedAction?.Invoke();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            var vw = this.ActualWidth / 2;
            var vh = this.ActualHeight / 2;
            RotationYCompositeTransform3D.CenterX = vw;
            RotationYCompositeTransform3D.CenterY = vh;
            var p = PieSlice.StrokeThickness / 2;
            PieSlice.TopCenter = new Point(vw, p);
            PieSlice.Radius = vw - p;
            PieSlice.SweepAngle = 0.0;
            PieSlice.SetClosedAndFilled(false);
            border.Width = 0;
            border1.Width = 0;
        }

        private void Init2()
        {
            // color
            var blue = BlueBrush.Color;
            ColorAnimation colorAnimation = new ColorAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(500),
                To = Color.FromArgb(blue.A, blue.R, blue.G, blue.B),
            };
            Storyboard.SetTarget(colorAnimation, PieSlice);
            Storyboard.SetTargetProperty(colorAnimation, "(Shape.Fill).(SolidColorBrush.Color)");
            ScaleStoryboard.Children.Add(colorAnimation);
        }

        private void Sb_Completed(object sender, object e)
        {
            var flag = Math.Abs(RotationYCompositeTransform3D.RotationY) < 0.1;
            if (_isStoped && flag) // 停止了
            {
                BeginEndAnimation();
                return;
            }

            var sb = sender as Storyboard;
            var doubleAnimation = (sb.Children[0] as DoubleAnimation);
            
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
            PieSlice.SweepAngle = 359.999;
            PieSlice.SetClosedAndFilled(true);
 
            ScaleStoryboard.Begin();
        }
        #endregion

        
    }
}
