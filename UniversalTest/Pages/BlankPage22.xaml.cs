using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
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
 
        // 用release模式
        private   int _value;
        public BlankPage22()
        {
            this.InitializeComponent();
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
        }
    }
}
