using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Diagnostics;
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
    public sealed partial class BlankPage21 : Page
    {
        public BlankPage21()
        {
            this.InitializeComponent();
            Loaded += BlankPage21_Loaded;
        }

        private void BlankPage21_Loaded(object sender, RoutedEventArgs e)
        {

            ProcessDiagnosticInfo;

            ProcessCpuUsage;
            ProcessDiskUsage;
            ProcessMemoryUsage;


            MemoryManager +  AppMemoryReport + ProcessMemoryReport;



        }
    }
}
