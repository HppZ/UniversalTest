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
using UniversalTest.Control.ScrollViewer.Children;
using UniversalTest.Controller;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.ScrollViewer
{
    public sealed partial class ThreeImageViewer : UserControl
    {
        #region Fields
        private MainController _mainController;
        private List<ImageItem> _images;
        private const double IMAGE_WH_RATIO = 4.0 / 3; // 照片宽高比
        private const double SCALE_RATIO = 0.8; // 缩放比例
        private const double SIDE_DISTANCE_RATIO = 5.0 / 16; // 两边间距占Image的比例
        #endregion

        #region Ctor
        public ThreeImageViewer()
        {
            this.InitializeComponent();

            this.SizeChanged += ThreeImageViewer_SizeChanged;
            Loaded += ThreeImageViewer_Loaded;
        }
        #endregion

        #region Loaded
        private void ThreeImageViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }
        #endregion

        #region Init
        /// <summary>
        /// 初始化
        /// </summary>
        private async void Init()
        {
            _mainController = new MainController();
            await _mainController.Init();
            _images = _mainController.Source.ToList();

            for (int i = 0; i < CanvasContainer.Children.Count; i++)
            {
                var child = CanvasContainer.Children[i];
                child.RenderTransform = new CompositeTransform();
                child.RenderTransformOrigin = new Point(0.5, 0.5);
                (child as ViewerItem).Source = _images[i].PreviewImage;
                InitPosition(child);
            }
        }

        /// <summary>
        /// 更新子项位置
        /// </summary>
        private void InitPosition(UIElement element)
        {
            var center = CanvasContainer.ActualWidth / 2;
            var item = element as FrameworkElement;
            var trans = item.RenderTransform as CompositeTransform;
            if (item.Tag.ToString() == "0")
            {
                Canvas.SetLeft(element, center - item.ActualWidth / 2 - item.ActualWidth * SIDE_DISTANCE_RATIO);
                trans.ScaleX = SCALE_RATIO;
                trans.ScaleY = SCALE_RATIO;
            }
            else if (item.Tag.ToString() == "1")
            {
                Canvas.SetLeft(element, center - item.ActualWidth / 2);
            }
            else if (item.Tag.ToString() == "2")
            {
                Canvas.SetLeft(element, center - item.ActualWidth / 2 + item.ActualWidth * SIDE_DISTANCE_RATIO);
                trans.ScaleX = SCALE_RATIO;
                trans.ScaleY = SCALE_RATIO;
                Canvas.SetZIndex(item, -1);
            }
            else if (item.Tag.ToString() == "3")
            {
                item.Opacity = 0;
            }
        }
        #endregion

        #region Auto Layout
        private void ThreeImageViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateImagesSize(e.NewSize);
        }

        /// <summary>
        /// 更新子项大小
        /// </summary>
        private void UpdateImagesSize(Size newSize)
        {
            foreach (var child in CanvasContainer.Children)
            {
                var ele = (child as FrameworkElement);
                ele.Height = newSize.Height;
                ele.Width = ele.Height * IMAGE_WH_RATIO;
            }

            var childWidth = (CanvasContainer.Children[0] as ViewerItem).Width;
            CanvasContainer.Width = childWidth + SIDE_DISTANCE_RATIO * childWidth * 2;
        }

        #endregion

        #region Change
        /// <summary>
        /// 更新所有子项位置
        /// </summary>
        private void GotoPreOrNext()
        {
        }
        #endregion

        #region Interaction
        //-------------------------------------------------------------------
        private void Left_Tapped(object sender, RoutedEventArgs e)
        {

        }

        private void Right_Tapped(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
