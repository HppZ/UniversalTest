using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UniversalTest.Controller;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage6 : Page
    {
        private MainController _mainController;
        private ScrollViewer _scrollViewer;
        private ItemsWrapGrid _itemsWrapGrid;
        public BlankPage6()
        {
            this.InitializeComponent();
            _mainController = new MainController();
            this.DataContext = _mainController.Source;

            Loaded += BlankPage6_Loaded;
        }

        private async void BlankPage6_Loaded(object sender, RoutedEventArgs e)
        {
            await _mainController.Init();
        }


        private void ScrollViewer_OnLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer  = sender as ScrollViewer;
            _scrollViewer.ViewChanged += _scrollViewer_ViewChanged;
        }

        private void _scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate) // scroll completed
            {
                Debug.WriteLine(_itemsWrapGrid?.FirstVisibleIndex);
            }
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            _itemsWrapGrid = sender as ItemsWrapGrid;
        }
    }
}
