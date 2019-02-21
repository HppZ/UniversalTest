using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
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
    public sealed partial class BlankPage38 : Page
    {
        public BlankPage38()
        {
            this.InitializeComponent();
        }

        private void XamlRoot_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var rect = new Rect(0, 0, xamlEle.ActualWidth, xamlEle.ActualHeight);
            var rect1= xamlEle.TransformToVisual(Window.Current.Content).TransformBounds(rect);
            var rect2 = xamlEle.TransformToVisual(xamlRoot).TransformBounds(rect);
            var rect3 = xamlEle.TransformToVisual(xamlRoot2).TransformBounds(rect);


            Debug.WriteLine($"{rect1.X} {rect1.Y} {rect1.Width} {rect1.Height}");
            Debug.WriteLine($"{rect2.X} {rect2.Y} {rect2.Width} {rect2.Height}");
            Debug.WriteLine($"{rect3.X} {rect3.Y} {rect3.Width} {rect3.Height}");

            // output: 

            //739 480 280 140
            //295 280 280 140
            //-100 -200 200 400

        }

    }
}
