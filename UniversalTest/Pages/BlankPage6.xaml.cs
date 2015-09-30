using System;
using System.Collections.Generic;
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
        public BlankPage6()
        {
            this.InitializeComponent();
            _mainController = new MainController();
            this.DataContext = _mainController.Source;

            Loaded += BlankPage6_Loaded;
        }

        private void BlankPage6_Loaded(object sender, RoutedEventArgs e)
        {
            _mainController.Init();
        }
    }

    public class MyGridViewItem : GridViewItem
    {
        public MyGridViewItem()
        {
            IsSelected = true;
        }
    }
}
