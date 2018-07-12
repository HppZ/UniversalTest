using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
    public sealed partial class BlankPage32 : Page
    {
        public BlankPage32()
        {
            this.InitializeComponent();
            Root.AddHandler(TappedEvent, new TappedEventHandler(Root_tapped),true );
        }

        private void Root_tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("tapped");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("button  1 clicked");
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("button  2 clicked");
        }
    }
}
