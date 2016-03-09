using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Random random = new Random();
        byte[] b = new byte[3];
        public MainPage()
        {
            this.InitializeComponent();
        }

      
        private void Begin(object sender, TappedRoutedEventArgs e)
        {
            InfiniteProgressEle.Begin();
        }

        private void Stop(object sender, TappedRoutedEventArgs e)
        {
            InfiniteProgressEle.Stop();

        }

        // 打算做个导航, 仅仅是打算

        // BlankPage1 ----------- 圆球滚动条 触屏模式下自动切换到圆球便于手指拖动
        // BlankPage2 ----------- listview重排序控件
        // BlankPage3 ----------- 右键层级菜单
        // BlankPage4 ----------- slider 做 圆球滚动条 和 1 一样
        // BlankPage5 ----------- slider 与 scrollviewer  双向绑定 
        // BlankPage6 ----------- GridView绑定Uri
        // BlankPage7 ----------- 图片切换控件 自动从系统图库取图，点击页面直接上/下一张 直到崩溃 :)

        // BlankPage10 ----------- 元素相交 mouseover  && tapped
        // BlankPage11 ----------- gif / mediaelement
        // BlankPage12 ----------- 保存应用名到文件
        // BlankPage13 ----------- gif


    }
}
