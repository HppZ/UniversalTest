﻿using System;
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
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        // 打算做个导航, 仅仅是打算

        // BlankPage1 ----------- 圆球滚动条 触屏模式下自动切换到圆球便于手指拖动
        // BlankPage2 ----------- TextBox的 visualstate test
        // BlankPage3 ----------- 右键层级菜单
        // BlankPage4 ----------- slider 做 圆球滚动条 和 1 一样
        // BlankPage5 ----------- slider 与 scrollviewer  双向绑定 
        // BlankPage6 ----------- 自定义gridviewitem 而不自定义gridview
        // BlankPage7 ----------- 图片切换控件 自动从系统图库取图，点击页面直接上/下一张 直到崩溃 :)
    }
}
