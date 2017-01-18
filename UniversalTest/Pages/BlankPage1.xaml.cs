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

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        /// <summary>
        /// 圆球滚动条  包含两种方案 check commit记录
        /// </summary>


        #region internal elements 
        private ObservableCollection<ImageItem> _source;
        private MainController mainController;

        private ScrollBar _scrollBar;
        private BallScrollBar _ballScrollBar;
        private ScrollViewer _scrollViewer;
        private Thumb _verticalThumb;
        #endregion

        #region ctor
        public BlankPage1()
        {
            this.InitializeComponent();

            mainController = new MainController();
            _source = mainController.Source;
            this.DataContext = _source;

            this.SizeChanged += BlankPage1_SizeChanged;
            Loaded += BlankPage1_Loaded;
        }
        #endregion

        #region loaded
        /// <summary>
        /// 圆球滚动条
        /// </summary>
        private void BallScrollBar_Loaded(object sender, RoutedEventArgs e)
        {
            _ballScrollBar = sender as BallScrollBar;
        }

        private void BlankPage1_Loaded(object sender, RoutedEventArgs e)
        {
            mainController.Init();
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = sender as ScrollViewer;
            _scrollViewer.ViewChanging += _scrollViewer_ViewChanging;
            _scrollViewer.ViewChanged += _scrollViewer_ViewChanged;

        }
        #endregion

        #region scrollviewer event
        private void _scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

            _ballScrollBar.SetValue(_scrollViewer.VerticalOffset);
        }

        private void _scrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
        }
        #endregion

        #region changed
        private void RangeBase_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            _scrollViewer.ChangeView(null, e.NewValue, 1);
        }

        private void BlankPage1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
        #endregion

        #region 调整滚动条
        
        #endregion

        #region tapped
        private void Test_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        #endregion

        private void ScrollBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Debug.WriteLine("real " + e.NewValue);
        }
    }
}
