using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Temp
{
    public sealed partial class MyUserControl2 : UserControl
    {
        public MyUserControl2()
        {
            this.InitializeComponent();
            Tapped += MyUserControl1_Tapped;
        }

        private void MyUserControl1_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var b =this.Resources["Color1"] as SolidColorBrush;
            b.Color = Colors.Green;
        }


        /*
         *无论使用merge dic与否，都没有share
         */


    }
}
