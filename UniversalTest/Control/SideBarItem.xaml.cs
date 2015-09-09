﻿using System;
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
        #endregion


        #region Private 
        #region Ani
        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DoAnimation();
        }

        /// <summary>
        /// 展开 收缩动画
        /// </summary>
        /// <param name="toOpen">true则强制展开</param>
        private void DoAnimation(bool? toOpen = null)
        {
            var sb = this.Resources["StoryboardForHeight"] as Storyboard;
            var da = sb.Children[0] as DoubleAnimation;

            if (toOpen != null)
                _isOpen = toOpen.Value;

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
            sb.Begin();
        }

        /// <summary>
        /// 使子项变为非选择态
        /// </summary>
        private void UnSelecteChildren()
        {
            ChildrenListView.SelectedItem = null;
        }

        #endregion
        #endregion
    }
}
