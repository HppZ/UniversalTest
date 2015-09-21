using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Windows.UI.Xaml.Navigation;
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
        private ScrollViewer _scrollViewer;
        private Thumb _verticalThumb;
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
            ResetViewportSize();
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
            if (_scrollBar != null && _scrollViewer != null)
            {
                double viewportsize = _scrollViewer.ViewportHeight;
                var shouldHeight = viewportsize * viewportsize / (_scrollViewer.ScrollableHeight + viewportsize);
                return shouldHeight;
            }
            return 0;
        }

        private void ResetViewportSize()
        {
            if (_scrollBar != null && _scrollViewer != null && _verticalThumb != null)
            {
                if (_scrollBar.IndicatorMode == ScrollingIndicatorMode.TouchIndicator)
                {
                    _scrollBar.ViewportSize = _scrollViewer.ViewportHeight;
                    return;
                };

                var shouldHeight = GetThumbHeight();
                if (Math.Abs(shouldHeight) < 0.01) return;
                var finalViewportSize = _verticalThumb.ActualHeight * _scrollViewer.ViewportHeight / shouldHeight;
                _scrollBar.ViewportSize = finalViewportSize - (shouldHeight - _verticalThumb.ActualHeight);
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
    }
}
