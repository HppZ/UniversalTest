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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xaml.Interactions.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage24 : Page
    {
        public BlankPage24()
        {
            this.InitializeComponent();
            var vm = new ViewModel1();
            ;
            this.DataContext = vm;
            vm.Url = new Uri("http://www.baidu.com");

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void WebView1_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
        }

        private void WebView1_OnFrameNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
        }

        private void WebView1_OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            
        }
    }


    class ViewModel1:ObservableObject
    {
        private RelayCommand<object> loginCommand;
        public RelayCommand<object> LoginCommand =>
            loginCommand ?? (loginCommand = new RelayCommand<object>((obj) =>
            {

            }));

        private Uri url;

        public Uri Url
        {
            get { return url; }
            set
            {
                Set(ref url, value);
            }
        }

       
    }
}
