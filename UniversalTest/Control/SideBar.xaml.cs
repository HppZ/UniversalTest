using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control
{
    public sealed partial class SideBar : UserControl
    {
        private ObservableCollection<string> photosMenuSource;
        private ObservableCollection<string> albumsMenuSource;
        private ObservableCollection<string> devicesMenuSource;

        public SideBar()
        {
            this.InitializeComponent();
            this.Loaded += SideBar_Loaded;
        }

        private void SideBar_Loaded(object sender, RoutedEventArgs e)
        {
            photosMenuSource = new ObservableCollection<string>()
            {
                "时刻",
                "地点",
                "人物",
                "全部",
            };
            PhotosItem.DataContext = photosMenuSource;

            albumsMenuSource = new ObservableCollection<string>()
            {
                "成都生活",
                "老家",
                "纽约",
                "大圣归来",
                "你",
                "我",
            };

            AlbumsItem.DataContext = albumsMenuSource;

            devicesMenuSource = new ObservableCollection<string>()
            {
                "Windows10-930",
                "Windows8.1-950",
                "iPhone",
                "XiaoMi",
            };

            DevicesItem.DataContext = devicesMenuSource;

        }
    }
}
