using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using UniversalTest.Helper;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sight.Windows10.Views.Album
{
    public partial class BallScrollBar : UserControl
    {
        #region internal element
        private Thumb _verticalThumb; // 滚动条内滑块
        private ScrollingIndicatorMode _indicatorMode = ScrollingIndicatorMode.MouseIndicator; // 用户输入模式
        private Timer _timer; // 延迟隐藏的计时器
        private ScrollViewer _scrollViewer; // 与此交互的scrollviewer
        private bool _isMouseOver; // 记录是否指针悬浮
        private bool _transitionTypeEnabled = true; //指示是否已经使用过渡样式
        private Storyboard _storyboard; // 滚动条高度动画
        #endregion

        #region ctor
        public BallScrollBar()
        {
            this.InitializeComponent();
            this.Loaded += BallScrollBar_Loaded;
            this.Unloaded += BallScrollBar_Unloaded;
            PointerExited += BallScrollBar_PointerExited;
            PointerMoved += BallScrollBar_PointerMoved;
            PointerEntered += BallScrollBar_PointerEntered;
            Root.Opacity = 0; // 默认隐藏
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
            if (!IsSupportTouchMode) return; // 如果不支持触摸直接返回

            string resource = "";
            if (mode == UserInteractionMode.Touch)
            {
                resource = "VerticalThumbStyle";
                _indicatorMode = ScrollingIndicatorMode.TouchIndicator;
                OnPointerOver(false, false, true);
            }
            else
            {
                resource = "DefaultVerticalThumbStyle";
                _indicatorMode = ScrollingIndicatorMode.MouseIndicator;
            }

            if (_verticalThumb == null) return;
            Style style = this.Resources[resource] as Style;
            if (_verticalThumb.Style != style)
                _verticalThumb.Style = style;
        }

        /// <summary>
        /// 重新计算滑块高度
        /// </summary>
        private void ReCalThumbSize()
        {
            if (_verticalThumb == null || _indicatorMode == ScrollingIndicatorMode.TouchIndicator) return;

            if (_storyboard == null)// 初始化动画
            {
                _storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation
                {
                    Duration = TimeSpan.FromMilliseconds(350),
                    EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut, Exponent = 3 },
                    EnableDependentAnimation = true
                };
                Storyboard.SetTarget(doubleAnimation, _verticalThumb);
                Storyboard.SetTargetProperty(doubleAnimation, "(FrameworkElement.Height)");
                _storyboard.Children.Add(doubleAnimation);
            }

            double to = (ViewportSize / (Maximum + ViewportSize)) * ViewportSize;
            ((DoubleAnimation)_storyboard.Children[0]).To = to;
            _storyboard.Completed += (e1, e2) =>
            {
                _verticalThumb.Height = (ViewportSize / (Maximum + ViewportSize)) * ViewportSize;
            };
            _storyboard.Begin();
        }

        #region 指针事件
        /// <summary>
        /// 指针离开滚动条
        /// </summary>
        private void BallScrollBar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _isMouseOver = false;
            if (Tag != null && Tag.Equals("PhotoGridView"))
            {
                _transitionTypeEnabled = true;
            }

            // 延时隐藏
            StartTimer();
        }

        /// <summary>
        /// 指针进入
        /// </summary>
        private async void BallScrollBar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _isMouseOver = true;
            await Task.Delay(50); // 延时显示
            if (_isMouseOver)
            {
                OnPointerOver(true);
            }
        }

        /// <summary>
        /// 指针移动
        /// </summary>
        private void BallScrollBar_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(this).Position;
            var flag = p.X < 0 || p.X > ActualWidth || p.Y < 0 || p.Y > ActualHeight; // 超出屏幕
            _isMouseOver = !flag; // 更新当前是否悬浮
            StartTimer(); // 延时隐藏
        }


        /// <summary>
        /// 指针over
        /// force 强制设置
        /// </summary>
        private void OnPointerOver(bool isOver, bool useTransition = true, bool force = false)
        {
            if (force || _indicatorMode == ScrollingIndicatorMode.MouseIndicator)
            {
                VisualStateManager.GoToState(this, isOver ? "PointerOver1" : "Normal1", useTransition);

                if (_verticalThumb != null)
                {
                    VisualStateManager.GoToState(_verticalThumb, isOver ? "PointerOver1" : "Normal1", useTransition);
                }
            }
        }

        #endregion

        /// <summary>
        /// slider值改变
        /// </summary>
        private void OnSliderValueChanged(double newValue)
        {
            if (_verticalThumb.IsDragging)
            {
                if (_transitionTypeEnabled && Tag != null && Tag.Equals("PhotoGridView"))
                {
                    _transitionTypeEnabled = false;
                }
                Scroll(newValue);
            }
            Show();
        }

        /// <summary>
        /// 设置slider的值
        /// </summary>
        private void SetSliderValue()
        {
            if (!_verticalThumb.IsDragging)
            {
                SliderElement.Value = _scrollViewer.VerticalOffset;
            }
        }
        #endregion

        #region virtual
        /// <summary>
        /// 获取ballscrollbar交互目标（scrollviewer）
        /// </summary>
        protected virtual void GetScrollViewer()
        {
            var sp = VisualTreeHelper.GetChild(this.Parent, 0) as ScrollContentPresenter;
            _scrollViewer = sp.ScrollOwner as ScrollViewer;
            _scrollViewer.ViewChanging -= _scrollViewer_ViewChanging;
            _scrollViewer.ViewChanging += _scrollViewer_ViewChanging;
            _scrollViewer.PointerMoved -= _scrollViewer_PointerMoved;
            _scrollViewer.PointerMoved += _scrollViewer_PointerMoved;
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
        /// 视图高度
        /// </summary>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register(
            "ViewportSize", typeof(double), typeof(BallScrollBar), new PropertyMetadata(default(double), OnViewportSizeCallback));

        private static void OnViewportSizeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BallScrollBar)d).ReCalThumbSize();
        }

        public double ViewportSize
        {
            get { return (double)GetValue(ViewportSizeProperty); }
            set { SetValue(ViewportSizeProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(BallScrollBar), new PropertyMetadata(default(double), OnValueChangedCallback));

        private static void OnValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BallScrollBar)d).SetSliderValue();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool IsSupportTouchMode { get; set; } = true; // 是否支持触摸模式
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
            CheckUserInteractionMode();
            GetScrollViewer();
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void BallScrollBar_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }
        #endregion

        #region something changed
        /// <summary>
        /// 滚动条值发生变化
        /// </summary>
        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            OnSliderValueChanged(e.NewValue);
        }

        /// <summary>
        /// 大小变化
        /// </summary>
        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            CheckUserInteractionMode();
        }

        #region scrollViewer
        /// <summary>
        /// 视图变化
        /// </summary>
        private void _scrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            Show();
        }

        /// <summary>
        /// 指针移动
        /// </summary>
        private void _scrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Show();
        }
        #endregion

        /// <summary>
        /// 点击滚动条滚动视图
        /// </summary>
        private void Slider_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Scroll(SliderElement.Value);
        }

        /// <summary>
        /// 滚动界面
        /// </summary>
        private void Scroll(double value)
        {
            _scrollViewer.ChangeView(null, value, null, true);
        }
        #endregion

        #region auto show & collapse
        /// <summary>
        /// 显示滚动条
        /// </summary>
        /// <param name="toShow"></param>
        public void Show(bool toShow = true)
        {
            if (toShow)
            {
                IsHitTestVisible = true;
                Root.Opacity = 1;
                StartTimer();
            }
            else
            {
                IsHitTestVisible = false;
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
            try
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    IsHitTestVisible = false;
                    //if (_isMouseOver) return;
                    OnPointerOver(false, false);
                    var sb = new Storyboard();
                    StoryboardHelper.CreatAnimation(Root, sb, "Opacity", 150, 0, new QuadraticEase() { EasingMode = EasingMode.EaseIn }, true);
                    sb.Begin();
                });
            }
            catch
            {
            }
        }
        #endregion
    }
}
