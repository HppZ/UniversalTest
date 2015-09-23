using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Sight.Windows10.Helper;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.ScrollBar
{
    public sealed partial class BallScrollBar : UserControl
    {
        #region internal element
        private Thumb _verticalThumb; // 滚动条内滑块
        private ScrollingIndicatorMode _indicatorMode; // 用户输入模式
        private ScrollViewer _scrollViewer; // 滚动条所在的scrollviewer
        private Timer _timer; // 延迟隐藏的计时器

        #endregion

        #region Event
        public event RangeBaseValueChangedEventHandler ValueChanged; // 滚动条滚动值变化
        #endregion

        #region ctor
        public BallScrollBar()
        {
            this.InitializeComponent();

            this.Loaded += BallScrollBar_Loaded;
            this.SizeChanged += This_SizeChanged;
        }

        #endregion

        #region public method
        /// <summary>
        /// 获取滑块应有高度
        /// </summary>
        /// <returns></returns>
        public double GetThumbActualHeight()
        {
            return _verticalThumb.ActualHeight;
        }

        /// <summary>
        /// 重新绑定滑块的值
        /// </summary>
        /// <param name="value"></param>
        public void ReBind(double value)
        {
            var bindingex = SliderElement.GetBindingExpression(RangeBase.ValueProperty);
            if (bindingex != null)
            {
                return;
            }

            var binding = new Binding()
            {
                Path = new PropertyPath("Value"),
                ElementName = "This"
            };
            
            SliderElement.SetBinding(RangeBase.ValueProperty, binding);
        }

        /// <summary>
        /// 设置当前的值
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(double value)
        {
            ReBind(value);
        }

        #endregion

        #region private method
        /// <summary>
        /// 检查用户输入模式
        /// </summary>
        private void CheckUserInteractionMode()
        {
            var mode = UIViewSettings.GetForCurrentView().UserInteractionMode;
            ChangeStyle(mode);
        }

        /// <summary>
        /// 切换滑块样式
        /// </summary>
        /// <param name="mode"></param>
        private void ChangeStyle(UserInteractionMode mode)
        {
            string resource = "";
            if (mode == UserInteractionMode.Touch)
            {
                resource = "VerticalThumbStyle";
                _indicatorMode = ScrollingIndicatorMode.TouchIndicator;
            }
            else if (mode == UserInteractionMode.Mouse)
            {
                resource = "DefaultVerticalThumbStyle";
                _indicatorMode = ScrollingIndicatorMode.MouseIndicator;
            }

            if (_verticalThumb == null) return;
            Style style = this.Resources[resource] as Style;
            _verticalThumb.Style = style;
        }

        private void ReCalThumbSize()
        {
            if(_verticalThumb == null)return;

            var h = (ViewportSize / (Maximum + ViewportSize)) * ViewportSize;
            _verticalThumb.Height = h;
            Debug.WriteLine(h);
        }

        #endregion

        #region public properties
        /// <summary>
        /// 方向
        /// </summary>
        public Orientation Orientation
        {
            set { SliderElement.Orientation = value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(Double), typeof(BallScrollBar), new PropertyMetadata(default(Double), OnMaximumCallback));

        private static void OnMaximumCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as BallScrollBar;
            c.ReCalThumbSize();
        }

        public Double Maximum
        {
            get { return (Double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(BallScrollBar), new PropertyMetadata(default(double)));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// 视图高度
        /// </summary>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register(
            "ViewportSize", typeof(double), typeof(BallScrollBar), new PropertyMetadata(default(double), OnViewportSizeCallback));

        private static void OnViewportSizeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as BallScrollBar;
            c.ReCalThumbSize();
        }

        public double ViewportSize
        {
            get { return (double)GetValue(ViewportSizeProperty); }
            set { SetValue(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// 用户输入模式
        /// </summary>
        public ScrollingIndicatorMode IndicatorMode => this._indicatorMode;

        #endregion

        #region Loaded
        /// <summary>
        ///  垂直滚动条加载完成
        /// </summary>
        private void VerticalThumb_OnLoaded(object sender, RoutedEventArgs e)
        {
            _verticalThumb = sender as Thumb;

            if (_indicatorMode == ScrollingIndicatorMode.TouchIndicator)
            {
                CheckUserInteractionMode();
            }

            ReCalThumbSize();
        }

        /// <summary>
        /// 滚动条加载完成
        /// </summary>
        private void BallScrollBar_Loaded(object sender, RoutedEventArgs e)
        {
            var p = this.Parent as FrameworkElement;
            _scrollViewer = VisualTreeExtensions.FindFirstElementInVisualTree<ScrollViewer>(p);
            if (_scrollViewer == null) throw new Exception("incorrect usage");
        }

        #endregion

        #region something changed
        /// <summary>
        /// 滚动条值发生变化
        /// </summary>
        private void ScrollBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Show();
            ValueChanged?.Invoke(sender, e);
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Show();

            Debug.WriteLine("fake "+e.NewValue);

            ValueChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// 大小变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void This_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckUserInteractionMode();
            ResetViewportSize();
        }

        /// <summary>
        /// 指针进入滚动条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollBar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_indicatorMode == ScrollingIndicatorMode.MouseIndicator)
                VisualStateManager.GoToState(SliderElement, "PointerEntered", true);
        }

        /// <summary>
        /// 指针离开滚动条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollBar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (_indicatorMode == ScrollingIndicatorMode.MouseIndicator)
                VisualStateManager.GoToState(SliderElement, "PointerExited", true);
        }

        /// <summary>
        /// 指针在滚动条内移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollBar_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Show();
        }
        #endregion

        #region adjust viewportsize
        /// <summary>
        /// 获取应有的高度
        /// </summary>
        private double GetThumbNaturalHeight()
        {
            if (_scrollViewer != null)
            {
                double viewportsize = _scrollViewer.ViewportHeight;
                var shouldHeight = viewportsize * viewportsize / (_scrollViewer.ScrollableHeight + viewportsize);
                return shouldHeight;
            }
            return 0;
        }

        /// <summary>
        /// 重置scrollbar的viewportsize
        /// </summary>
        private void ResetViewportSize()
        {
            return;
            if (_scrollViewer != null)
            {
                if (this.IndicatorMode == ScrollingIndicatorMode.MouseIndicator)
                {
                    this.ViewportSize = _scrollViewer.ViewportHeight;
                    return;
                };

                var shouldHeight = GetThumbNaturalHeight(); // 应有高度
                if (Math.Abs(shouldHeight) < 0.01) return;
                var thumbHeight = this.GetThumbActualHeight(); // 实际高度
                var finalViewportSize = thumbHeight * _scrollViewer.ViewportHeight / shouldHeight;
                this.ViewportSize = finalViewportSize - (shouldHeight - thumbHeight);
            }
        }
        #endregion

        #region 隐藏 显示
        /// <summary>
        /// 显示滚动条
        /// </summary>
        /// <param name="toShow"></param>
        public void Show(bool toShow = true)
        {
            if (toShow)
            {
                Root.Opacity = 1;
                StartTimer();
            }
            else
            {
                Root.Opacity = 0;
            }
        }

        /// <summary>
        /// 计时器启动
        /// </summary>
        private void StartTimer()
        {
            int dueTime = 2000;
            if (_timer == null)
                _timer = new Timer(TimerCompleted, null, dueTime, -1);
            else
            {
                _timer.Change(dueTime, Timeout.Infinite);
            }
        }

        /// <summary>
        /// 计时器到时
        /// </summary>
        private async void TimerCompleted(object state)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Root.Opacity = 0;
                //var sb = new Storyboard();
                //StoryboardHelper.CreatAnimation(Root, sb, "Opacity", 150, 0, new QuadraticEase() { EasingMode = EasingMode.EaseIn }, true);
                //sb.Begin();
            });
        }

        #endregion

        
    }
}
