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
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<PageInfo> pageInfos = new List<PageInfo>
            {
                new PageInfo("BlankPage1", "圆球滚动条"),
                new PageInfo("BlankPage2", "listview重排序控件"),
                new PageInfo("BlankPage3", "右键层级菜单"),
                new PageInfo("BlankPage4", "slider 做 圆球滚动条 和 1 一样"),
                new PageInfo("BlankPage5", "slider 与 scrollviewer 双向绑定 "),
                new PageInfo("BlankPage6", "GridView绑定Uri"),
                new PageInfo("BlankPage7", "图片切换控件"),
                new PageInfo("BlankPage8", "空"),
                new PageInfo("BlankPage9", "CustomPanel"),
                new PageInfo("BlankPage10", "元素相交 mouseover && tapped"),
                new PageInfo("BlankPage11", "gif / mediaelement"),
                new PageInfo("BlankPage12", "保存应用名到文件"),
                new PageInfo("BlankPage13", "gif"),
                new PageInfo("BlankPage14", "InfiniteProgress"),
                new PageInfo("BlankPage15", "转盘"),
                new PageInfo("BlankPage16", "测试虚拟化"),
                new PageInfo("BlankPage17", "分组"),
                new PageInfo("BlankPage18", "dragslider"),
                new PageInfo("BlankPage19", "sqlite multi-thread test"),
                new PageInfo("BlankPage20", "animation & images"),
                new PageInfo("BlankPage21", "cpu / memory / disk info"),
                new PageInfo("BlankPage22", "primitive type & thread safety"),
                new PageInfo("BlankPage23", "richtextblock"),
                new PageInfo("BlankPage24", "binding webview navigationargs's url "),
                new PageInfo("BlankPage25", "c# .NET Test"),
                new PageInfo("BlankPage26", "property changed event & ui thread?"),
                new PageInfo("BlankPage27", "download svg from icon8"),
                new PageInfo("BlankPage28", "image collapse failed event occur?"),
                new PageInfo("BlankPage29", "image x;bind string null empty whitespace"),
                new PageInfo("BlankPage30", "storyboard test"),
                new PageInfo("BlankPage31", "rename images"),




                new PageInfo("Test", "Test"),
            };
            pageInfos.Reverse();

            listviewElement.ItemsSource = pageInfos;
        }

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var pageinfo = (sender as FrameworkElement).DataContext as PageInfo;
            if (pageinfo != null)
            {
                frameElement.Navigate(Type.GetType("UniversalTest.Pages." + pageinfo.PageName));
            }
        }
    }

    class PageInfo
    {
        public PageInfo(string name, string des)
        {
            PageName = name;
            Description = des;
        }

        public string PageName { get; set; }
        public string Description { get; set; }

    }

}
