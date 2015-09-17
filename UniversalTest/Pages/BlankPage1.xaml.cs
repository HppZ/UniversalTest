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
        private ObservableCollection<ImageItem> _source;
        private MainController mainController;

        private ScrollBar _scrollBar;
        private ScrollViewer _scrollViewer;
        public BlankPage1()
        {
            this.InitializeComponent();

            mainController = new MainController();
            _source = mainController.Source;
            this.DataContext = _source;

            this.SizeChanged += BlankPage1_SizeChanged;
            Loaded += BlankPage1_Loaded;
        }

        private void BlankPage1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_scrollBar != null && _scrollViewer != null)
            {
                double s = _scrollViewer.ViewportHeight;
                s = 30;
                double max = _scrollBar.Maximum;
                double min = _scrollBar.Minimum;
                double t = s*s - 4*(min - max);
                double b24ac = Math.Sqrt(t);
                var r = (b24ac + s) / 2;

                _scrollBar.ViewportSize = r;
                Debug.WriteLine("viewpostsize "+r);
            }
        }

        private void BlankPage1_Loaded(object sender, RoutedEventArgs e)
        {
            mainController.Init();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //Debug.WriteLine(_scrollViewer.ScrollableHeight);
            //Debug.WriteLine(_scrollBar.ViewportSize);
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = sender as ScrollViewer;
        }

        private void VerticalScrollBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            _scrollBar = sender as ScrollBar;
        }

    }
}
