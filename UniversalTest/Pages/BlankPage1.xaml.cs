using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using UniversalTest.Control.ScrollBar;
using UniversalTest.Controller;
using UniversalTest.Helper;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        /// <summary>
        /// 圆球滚动条
        /// </summary>

        private ObservableCollection<ImageItem> _source;
        private MainController mainController;

        private ScrollBar _scrollBar;
        private BallScrollBar _ballScrollBar;
        private ScrollViewer _scrollViewer;
        private Thumb _verticalThumb;
        private Slider _slider;
        public BlankPage1()
        {
            this.InitializeComponent();

            mainController = new MainController();
            _source = mainController.Source;
            this.DataContext = _source;

            this.SizeChanged += BlankPage1_SizeChanged;
            Loaded += BlankPage1_Loaded;
        }

        #region Loaded
        private void BlankPage1_Loaded(object sender, RoutedEventArgs e)
        {
            mainController.Init();
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = sender as ScrollViewer;
            _scrollViewer.ViewChanging += _scrollViewer_ViewChanging;
            _scrollViewer.ViewChanged += _scrollViewer_ViewChanged;

            ResetViewportSize();
        }

        private void _scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if(_slider == null)return;
            _slider.Value = _scrollViewer.VerticalOffset;
        }

        private void _scrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
        }

        private void VerticalScrollBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            _scrollBar = sender as ScrollBar;
            _scrollBar.ValueChanged += _scrollBar_ValueChanged;

            ResetViewportSize();
        }

        private void VerticalThumb_OnLoaded(object sender, RoutedEventArgs e)
        {
            _verticalThumb = sender as Thumb;
            ResetViewportSize();
        }
        #endregion

        #region Other
        private void BlankPage1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResetViewportSize();

            var mode = UIViewSettings.GetForCurrentView().UserInteractionMode;
            if (mode == UserInteractionMode.Touch)
            {
                Debug.WriteLine("touch");
            }
            else if(mode == UserInteractionMode.Mouse)
            {
                Debug.WriteLine("mouse");
            }
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void _scrollBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
        }

        private void VerticalThumb_OnDragStarted(object sender, DragStartedEventArgs e)
        {
            Debug.WriteLine("started");
        }

        private void VerticalThumb_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("tapped");
        }
        #endregion


        #region 调整滚动条
        private double GetThumbHeight()
        {
            if (_scrollViewer != null)
            {
                double viewportsize = _scrollViewer.ViewportHeight;
                var shouldHeight = viewportsize * viewportsize / (_scrollViewer.ScrollableHeight + viewportsize);
                return shouldHeight;
            }
            return 0;
        }

        private void ResetViewportSize()
        {
            if ( _scrollViewer != null && _ballScrollBar !=null)
            {
                if (_ballScrollBar.IndicatorMode == ScrollingIndicatorMode.MouseIndicator)
                {
                    _ballScrollBar.ViewportSize = _scrollViewer.ViewportHeight;
                    return;
                };

                var shouldHeight = GetThumbHeight();
                if (Math.Abs(shouldHeight) < 0.01) return;
                var thumbHeight = _ballScrollBar.GetThumbHeight();
                var finalViewportSize = thumbHeight * _scrollViewer.ViewportHeight / shouldHeight;
                _ballScrollBar.ViewportSize = finalViewportSize - (shouldHeight - thumbHeight);
            }
        }
        #endregion

        private void Test_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void VerticalPanningRoot_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("Size " + e.NewSize);
        }

        private void RangeBase_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Debug.WriteLine("offset "+_scrollViewer.VerticalOffset);
            Debug.WriteLine("value "+e.NewValue);

            _scrollViewer.ChangeView(null,e.NewValue,1);
        }

        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            _slider = sender as Slider;
        }

        private void BallScrollBar_Loaded(object sender, RoutedEventArgs e)
        {
            _ballScrollBar = sender as BallScrollBar;
        }
    }
}
