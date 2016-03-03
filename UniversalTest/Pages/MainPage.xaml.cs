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
        
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = 359.9,
                Duration = TimeSpan.FromMilliseconds(3000),
                EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut}
            };

            Storyboard sb  =new Storyboard() ;
            sb.Completed += Sb_Completed;
            Storyboard.SetTarget(doubleAnimation, PieSlice);
            Storyboard.SetTargetProperty(doubleAnimation, "SweepAngle");
            sb.Children.Add(doubleAnimation);
            sb.Begin();
        }

        private void Sb_Completed(object sender, object e)
        {
            var sb = sender as Storyboard;
            var doubleAnimation = (sb.Children[0] as DoubleAnimation);

            var flag = Math.Abs(RotationYCompositeTransform3D.RotationY) < 0.1;
            if (flag) // blue for now
            {
                RotationYCompositeTransform3D.RotationY = 180;
                //PieSlice.Stroke = WhiteBrush;
                doubleAnimation.From = 359.9;
                doubleAnimation.To = 0;
            }
            else
            {
                RotationYCompositeTransform3D.RotationY = 0;
                //PieSlice.Stroke = BlueBrush;
                doubleAnimation.From = 0;
                doubleAnimation.To = 359.9;
            }

            sb?.Begin();
        }


        // BlankPage1 ----------- 圆球滚动条
        // BlankPage2 ----------- TextBox的 visualstate
        // BlankPage3 ----------- 右键菜单 (popup)
        // BlankPage4 ----------- slider 做 圆球滚动条
        // BlankPage5 ----------- slider 与 scrollviewer  双向绑定
        // BlankPage6 ----------- 自定义gridviewitem 而不自定义gridview


    }
}


