using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniversalTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// 目录
        /// </summary>
        
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            T1();
        }

        private async void T1()
        {
           await ThreadPool.RunAsync(async (e1) =>
            {
               await T2();
                Debug.WriteLine("after t2");
            }, WorkItemPriority.Low);
        }

        private async Task T2()
        {
            List<Task> request = new List<Task>();
            for (int i = 0; i < 12; i++)
            {
                var r = Task.Delay(1000);
                request.Add(r);
                if ((i + 1)%3 == 0)
                {
                    Debug.WriteLine("before delay " + i);
                    await Task.WhenAll(request);
                    Debug.WriteLine("after delay " + i);
                }
            }
            Debug.WriteLine("Done");
        }

        // 打算做个导航, 仅仅是打算

        // BlankPage1 ----------- 圆球滚动条 触屏模式下自动切换到圆球便于手指拖动
        // BlankPage2 ----------- listview重排序控件
        // BlankPage3 ----------- 右键层级菜单
        // BlankPage4 ----------- slider 做 圆球滚动条 和 1 一样
        // BlankPage5 ----------- slider 与 scrollviewer  双向绑定 
        // BlankPage6 ----------- GridView绑定Uri
        // BlankPage7 ----------- 图片切换控件 自动从系统图库取图，点击页面直接上/下一张 直到崩溃 :)
    }
}
