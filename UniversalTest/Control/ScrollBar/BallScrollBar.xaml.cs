using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.ScrollBar
{
    public sealed partial class BallScrollBar : UserControl
    {
        #region internal element
        private Thumb _verticalThumb;
        private ScrollingIndicatorMode _indicatorMode;
        #endregion

        #region Event
        public event RangeBaseValueChangedEventHandler ValueChanged;
        #endregion

        #region ctor
        public BallScrollBar()
        {
            this.InitializeComponent();
            this.SizeChanged += This_SizeChanged;
        }
        #endregion

        #region public method
        public double GetThumbHeight()
        {
            return _verticalThumb.ActualHeight;
        }
        #endregion

        #region private method
        private void ChangeMode()
        {
            var mode = UIViewSettings.GetForCurrentView().UserInteractionMode;
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
        #endregion

        #region public properties
        public Orientation Orientation
        {
            set { ScrollBarElement.Orientation = value; }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(Double), typeof(BallScrollBar), new PropertyMetadata(default(Double)));
        public Double Maximum
        {
            get { return (Double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(BallScrollBar), new PropertyMetadata(default(double)));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register(
            "ViewportSize", typeof(double), typeof(BallScrollBar), new PropertyMetadata(default(double)));

        public double ViewportSize
        {
            get { return (double)GetValue(ViewportSizeProperty); }
            set { SetValue(ViewportSizeProperty, value); }
        }

        public ScrollingIndicatorMode IndicatorMode => this._indicatorMode;

        #endregion

        #region Loaded
        /// <summary>
        ///  垂直滚动条Thumb
        /// </summary>
        private void VerticalThumb_OnLoaded(object sender, RoutedEventArgs e)
        {
            _verticalThumb = sender as Thumb;
        }
        #endregion

        #region something changed
        /// <summary>
        /// 滚动条值发生变化
        /// </summary>
        private void ScrollBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        private void This_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeMode();
        }
        #endregion
    }
}
