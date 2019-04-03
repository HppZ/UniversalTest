using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{

    public sealed partial class BlankPage17 : Page
    {

        public BlankPage17()
        {
            this.InitializeComponent();
            Loaded += BlankPage17_Loaded;


        }

        private void BlankPage17_Loaded(object sender, RoutedEventArgs e)
        {
            var r = new List<A>();
            for (int index = 0; index < 10; index++)
            {
                var a = new A()
                {
                    Key = index.ToString(),
                };
                for (int i = 0; i < 5; i++)
                {
                    a.Add(new B()
                    {
                        Name = i + " data"
                    });
                }
                r.Add(a);
            }
            CollectionViewSource.Source = r;

        }
    }

    class A : List<B>
    {
        public string Key { get; set; }
    }

    class B
    {
        public string Name { get; set; }
    }

}
