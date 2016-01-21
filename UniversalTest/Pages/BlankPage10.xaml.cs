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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage10 : Page
    {
        public BlankPage10()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Enter " + (sender as FrameworkElement).Tag);
        }

        private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Exited " + (sender as FrameworkElement).Tag);
        }

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Tapped " + (sender as FrameworkElement).Tag);
        }
    }
}
