using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage40 : Page
    {
        public BlankPage40()
        {
            this.InitializeComponent();
            xamlGridView.ItemsSource = Enumerable.Range(0, 100);
            xamlGridView.AddHandler(PointerPressedEvent, new PointerEventHandler(UIElement_OnPointerPressed), true);
        }

        private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("GridViewT 1");
        }




    }



    class GridViewT : GridView
    {
        public GridViewT()
        {
            Loaded += GridViewT_Loaded;
        }

        private void GridViewT_Loaded(object sender, RoutedEventArgs e)
        {
            this.AddHandler(PointerPressedEvent, new PointerEventHandler(UIElement_OnPointerPressed), true);
        }

        private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("GridViewT 2");
        }


    }

}
