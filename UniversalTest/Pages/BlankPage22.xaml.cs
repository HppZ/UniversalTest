﻿using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
=======
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
>>>>>>> 9705be669b2ea7e1d9863030f697260b4ba67751
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
<<<<<<< HEAD
using Windows.UI.Xaml.Documents;
=======
>>>>>>> 9705be669b2ea7e1d9863030f697260b4ba67751
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage22 : Page
    {
<<<<<<< HEAD
        public BlankPage22()
        {
            this.InitializeComponent();
             
        }
=======
        // 用release模式
        private   int _value;
        public BlankPage22()
        {
            this.InitializeComponent();
            Loaded += BlankPage22_Loaded;
        }

        private async void BlankPage22_Loaded(object sender, RoutedEventArgs e)
        {
            var task1 = Task.Run(() =>
              {
                  for (int i = 0; i < 100000; i++)
                  {
                      Interlocked.Increment(ref _value);
                  }
              });

            var task2 = Task.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    Interlocked.Increment(ref _value);

                }
            });

            var task3 = Task.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    Interlocked.Increment(ref _value);

                }
            });

            var task4 = Task.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    Interlocked.Increment(ref _value);

                }
            });

            var list = new List<Task>()
            {
                task1,task2,task3,task4
            };
             
            await Task.WhenAll(list);
            Block1.Text = _value.ToString();
        }


>>>>>>> 9705be669b2ea7e1d9863030f697260b4ba67751
    }
}
