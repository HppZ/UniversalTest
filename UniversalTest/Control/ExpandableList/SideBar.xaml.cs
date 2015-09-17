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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control
{
    public sealed partial class SideBar : UserControl
    {
        private ObservableCollection<string> photosMenuSource;
        private ObservableCollection<string> albumsMenuSource;
        private ObservableCollection<string> devicesMenuSource;


        private bool _isWide = true; // 是否处于宽模式
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


        /// <summary>
        /// 关闭所有items
        /// </summary>
        public void CloseAllItems()
        {
            var sb = new Storyboard();

            SwitchMode(sb);

            var r1 = PhotosItem.OpenChildrenAnimation(false, sb);
            var r2 = AlbumsItem.OpenChildrenAnimation(false, sb);
            var r3 = DevicesItem.OpenChildrenAnimation(false, sb);
            sb.Begin();
            return;

            if (r1 || r2 || r3)
            {
                
                sb.Completed += Sb_Completed;
            }
            else
            {
                SwitchMode();
            }
        }

        private void Sb_Completed(object sender, object e)
        {
            SwitchMode();
        }

        public void SwitchMode(Storyboard sb = null)
        {
            var shouldBegin = sb == null;
            if(sb == null)
                sb = new Storyboard();
            var da = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(da, RootGrid);
            Storyboard.SetTargetProperty(da, "(FrameworkElement.Width)");

            // 处于宽模式
            if (_isWide)
            {
                da.From = RootGrid.ActualWidth;
                da.To = 30;// TODO value
            }
            // 处于窄模式
            else
            {
                da.From = RootGrid.ActualWidth;
                da.To = ItemsListView.ActualWidth;
            }
            _isWide = !_isWide;
            sb.Children.Add(da);

            if(shouldBegin)
                sb.Begin();
        }


        public void ChangeSource()
        {
            albumsMenuSource.Remove(albumsMenuSource.Last());
            AlbumsItem.SetHeight();
        }
    }
}
