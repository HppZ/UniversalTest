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

        public static readonly DependencyProperty ItemWidth1Property = DependencyProperty.Register(
            "ItemWidth1", typeof(double), typeof(BlankPage17), new PropertyMetadata(default(double)));

        public double ItemWidth1
        {
            get { return (double) GetValue(ItemWidth1Property); }
            set { SetValue(ItemWidth1Property, value); }
        }

        public static readonly DependencyProperty ItemWidth2Property = DependencyProperty.Register(
            "ItemWidth2", typeof(double), typeof(BlankPage17), new PropertyMetadata(default(double)));

        public double ItemWidth2
        {
            get { return (double) GetValue(ItemWidth2Property); }
            set { SetValue(ItemWidth2Property, value); }
        }

        private void BlankPage17_Loaded(object sender, RoutedEventArgs e)
        {
            var r = new List<A>();

            var a1 = new A()
            {
                Key = "folders",
            };
            for (int i = 0; i < 5; i++)
            {
                a1.Add(new B1()
                {
                    Name = "folder " + i
                });
            }
            r.Add(a1);

            for (int index = 0; index < 10; index++)
            {
                var a = new A()
                {
                    Key = index.ToString(),
                };
                for (int i = 0; i < 5; i++)
                {
                    a.Add(new B2()
                    {
                        Name = i + " data"
                    });
                }
                r.Add(a);
            }

            CollectionViewSource.Source = r;
        }

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w = e.NewSize.Width;

            if (w > 1000)
            {
                ItemWidth1 = 500;
                ItemWidth2 = 400;
            }
            else
            {
                ItemWidth1 = 200;
                ItemWidth2 = 50;
            }
        }
    }

    class A : List<B>
    {
        public string Key { get; set; }
    }

    class B
    {

    }

    class B1 : B
    {
        public string Name { get; set; }
    }

    class B2 : B
    {
        public string Name { get; set; }
    }

    class containerStyleSelector : StyleSelector
    {
        public Style Style1 { get; set; }
        public Style Style2 { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if (item is B1)
                return Style1;

            return Style2;
        }
    }



}
