using System;
using System.Collections.Generic;
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
    public sealed partial class SideBarItem : UserControl
    {
        #region Fields
        public bool _isOpen; // 展开状态
        #endregion

        #region Ctor
        public SideBarItem()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Property
        /// <summary>
        /// 子项模板
        /// </summary>
        private DataTemplate itemDataTemplate;
        public DataTemplate ItemDataTemplate 
        {
            get
            {
                if (itemDataTemplate == null)
                {
                    return this.Resources["DefaultTemplateKey"] as DataTemplate; // 默认
                }
                return itemDataTemplate;
            }
            set { itemDataTemplate = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标资源
        /// </summary>
        public Uri IconUriSource { get; set; }

        public bool _isWide = true; // 宽窄模式
        public bool IsWide
        {
            get { return _isWide; }
            set { _isWide = value; }
        }
        #endregion


        #region Private 
        #region Ani
        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(_isWide)
                OpenChildrenAnimation();
        }

        /// <summary>
        /// 展开 收缩动画
        /// </summary>
        /// <param name="toOpen">true则强制展开</param>
        public bool OpenChildrenAnimation(bool? toOpen = null, Storyboard sb = null)
        {
            if (toOpen != null && toOpen.Value == _isOpen)
            {
                return false;
            }

            bool shouldBegin = sb == null;
            if (shouldBegin)
            {
                sb = new Storyboard();
            }

            var da = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(da, RootContainer);
            Storyboard.SetTargetProperty(da, "(FrameworkElement.Height)");

            //处于展开状态
            if (_isOpen)
            {
                da.To = 0;
                da.From = RootContainer.ActualHeight;
            }
            // 处于关闭状态
            else
            {
                
                da.To = SecondContainer.ActualHeight;
                da.From = RootContainer.ActualHeight;
            }
            _isOpen = !_isOpen;
            ArrowRotateTransform.Angle = _isOpen ? 90 : 0;
            sb.Children.Add(da);
            if(shouldBegin)
                sb.Begin();
            return true;
        }

        /// <summary>
        /// 使子项变为非选择态
        /// </summary>
        private void ClearSelectedItem()
        {
            ChildrenListView.SelectedItem = null;
        }

        /// <summary>
        /// 切换宽窄模式
        /// </summary>
        /// <param name="toWide">true 强制变成宽模式</param>
        /// <param name="sb">传sb是为了同步动画</param>
        public void SwitchMode(bool? toWide = null, Storyboard sb = null)
        {
            if (toWide != null && toWide.Value == _isWide)
            {
                return;
            }

            bool shouldBegin = sb == null;
            if(shouldBegin)
                sb = new Storyboard();

            var da = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut}
            };

            Storyboard.SetTarget(da,RootStackPanel);
            Storyboard.SetTargetProperty(da, "(FrameworkElement.Width)");

            // 处于宽模式
            if (_isWide)
            {
                da.From = RootStackPanel.ActualWidth;
                da.To = 30;// TODO value
            }
            // 处于窄模式
            else
            {
                da.From = RootStackPanel.ActualWidth;
                da.To = TitleGrid.ActualWidth;
            }
            _isWide = !_isWide;
            sb.Children.Add(da);
            if(shouldBegin)
                sb.Begin();
        }

        public void SwitchIcon()
        {
            
        }


        public void SetHeight()
        {
            if(_isOpen)
                RootContainer.Height = Double.NaN;
        }
        #endregion
        #endregion
    }
}
