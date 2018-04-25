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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage28 : Page
    {
        public BlankPage28()
        {
            this.InitializeComponent();

            try
            {
                Uri.TryCreate("", UriKind.RelativeOrAbsolute, out var uri1);
                new BitmapImage(uri1);// 异常
            }
            catch (Exception e)
            {
                Debug.Assert(false);
            }

            try
            {
                Uri.TryCreate("   ", UriKind.RelativeOrAbsolute, out var uri2);
                new BitmapImage(uri2);// 异常

            }
            catch (Exception e)
            {
                Debug.Assert(false);

            }

            try
            {
                Uri.TryCreate(null, UriKind.RelativeOrAbsolute, out var uri3);
                new BitmapImage(uri3);// 异常

            }
            catch (Exception e)
            {
                Debug.Assert(false);
            }

            // ----------------------------
            try
            {
                Image1.Source = null;

            }
            catch (Exception e)
            {
                Debug.Assert(false);
            }

            try
            {
                Image2.Source = new BitmapImage(new Uri(""));
            }
            catch (Exception e)
            {
                // 异常
                Debug.Assert(false);
            }

            try
            {
                Image3.Source = new BitmapImage(null);

            }
            catch (Exception e)
            {
                // 异常
                Debug.Assert(false);
            }

            try
            {
                Image5.Source = new BitmapImage(new Uri("  "));
            }
            catch (Exception e)
            {
                // 异常
                Debug.Assert(false);
            }

            try
            {
                Image5.Source = new BitmapImage(new Uri("http://pic7.qiyipic.com/image/20180418/2a/38/v_115667590_m_601_1080_608.jpg"));

            }
            catch (Exception e)
            {
                Debug.Assert(false);
            }

        }

        private void Image_OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                Debug.WriteLine("failed " + element.Tag);
                element.Visibility = Visibility.Collapsed;
            }
        }

        private void Image_OnImageOpened(object sender, RoutedEventArgs e)
        {

            if (sender is FrameworkElement element)
            {
                Debug.WriteLine("opened " + element.Tag);
                element.Visibility = Visibility.Visible;
            }
        }



    }
}
