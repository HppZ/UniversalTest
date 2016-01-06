using System;
using System.Collections.Generic;
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
using UniversalTest.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage8 : Page
    {
        private MainController _mainController;
        private RangeCollection _imageItems;
        public BlankPage8()
        {
            this.InitializeComponent();
            _mainController = new MainController();
            Loaded += BlankPage8_Loaded;
        }

        private async void BlankPage8_Loaded(object sender, RoutedEventArgs e)
        {
            await _mainController.Init();
            _imageItems = new RangeCollection(_mainController.Source);
            DataContext = _imageItems;
        }
    }
}
