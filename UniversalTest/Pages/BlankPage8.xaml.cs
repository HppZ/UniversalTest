using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using UniversalTest.Controller;
using UniversalTest.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage8 : Page
    {
        private ItemsWrapGrid _itemsWrapGrid;
        private MainController _mainController;
        private RangeCollection _imageItems;
        public static CoreDispatcher MainDispatcher;
        public BlankPage8()
        {
            this.InitializeComponent();
            MainDispatcher = this.Dispatcher;
            _mainController = new MainController();
            Loaded += BlankPage8_Loaded;
        }

        private async void BlankPage8_Loaded(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            var g = new GroupSource();
            int index = 0;
            for (int i = 0; i < 10; i++)
            {
                var item = new GroupSourceItem() { Key = i.ToString() };
                var count = r.Next(5, 20);
                var d = new ObservableCollection<int>();
                for (int j = 0; j < count; j++)
                {
                    d.Add(index);
                    index++;
                }
                item.Data = d;
                g.Add(item);
            }
            CollectionSource.Source = g;


            //await _mainController.Init();
            //_imageItems = _mainController.Source;
            //DataContext = _imageItems;
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            // var grid = sender as Grid;
            // var img = grid.Children[0] as Image;
            //var b =  img.Source as BitmapImage;
        }

        private void ScrollViewer_OnLoaded(object sender, RoutedEventArgs e)
        {
            var scrollviewer = sender as ScrollViewer;
            scrollviewer.ViewChanging += Scrollviewer_ViewChanging;
            scrollviewer.ViewChanged += Scrollviewer_ViewChanged;
        }

        private void Scrollviewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            Debug.WriteLine("ed");
            Debug.WriteLine("first  " + _itemsWrapGrid.FirstVisibleIndex);
            Debug.WriteLine("last  " + _itemsWrapGrid.LastVisibleIndex);
        }

        private void Scrollviewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            Debug.WriteLine("ing");
            Debug.WriteLine("first  " + _itemsWrapGrid.FirstVisibleIndex);
            Debug.WriteLine("last  " + _itemsWrapGrid.LastVisibleIndex);
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            _itemsWrapGrid = sender as ItemsWrapGrid;
        }
    }
}
