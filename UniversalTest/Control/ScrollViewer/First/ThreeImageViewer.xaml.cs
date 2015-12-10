using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using UniversalTest.Helper;

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
        private const double SIDE_DISTANCE_RATIO = 14.0 / 16; // 两边间距占Image的比例
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
                (child as FrameworkElement).DataContext = _images[i];
                InitPosition(child);
            }
        }

        /// <summary>
        /// 更新子项位置
        /// </summary>
        private void InitPosition(UIElement element)
        {
            var center = CanvasContainer.ActualWidth / 2;
            Debug.WriteLine("center " + center);

            var item = element as FrameworkElement;
            var trans = item.RenderTransform as CompositeTransform;
            if (item.Tag.ToString() == "0")
            {
                var v = center - item.ActualWidth/2 - item.ActualWidth*SIDE_DISTANCE_RATIO;
                Canvas.SetLeft(element, v);
                trans.ScaleX = SCALE_RATIO;
                trans.ScaleY = SCALE_RATIO;

                Debug.WriteLine("0 " + v);
            }
            else if (item.Tag.ToString() == "1")
            {
                var v = center - item.ActualWidth / 2;
                Canvas.SetLeft(element, v);
                Canvas.SetZIndex(item, 1);

                Debug.WriteLine("1 " + v);
            }
            else if (item.Tag.ToString() == "2")
            {
                var v = center - item.ActualWidth / 2 + item.ActualWidth * SIDE_DISTANCE_RATIO;
                Canvas.SetLeft(element, v);
                trans.ScaleX = SCALE_RATIO;
                trans.ScaleY = SCALE_RATIO;

                Debug.WriteLine("2 " + v);
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

            var childWidth = (CanvasContainer.Children[0] as FrameworkElement).Width;
            CanvasContainer.Width = childWidth + SIDE_DISTANCE_RATIO * childWidth * 2;
        }

        #endregion

        #region Change
        /// <summary>
        /// 更新所有子项位置
        /// </summary>
        private void GotoPreOrNext(bool toNext)
        {
            var img0 = GetItemByTag(0);
            var img1 = GetItemByTag(1);
            var img2 = GetItemByTag(2);
            var img3 = GetItemByTag(3);

            var center = CanvasContainer.ActualWidth / 2;
            var duration = 600;
            var beginTime = 80;
            Storyboard storyboard = new Storyboard();
            var ease = new QuarticEase(){EasingMode = EasingMode.EaseOut};

            // img0
            StoryboardHelper.CreatAnimation(img0, storyboard, "UIElement.Opacity", duration, 0, 0, null, false);

            // img1
            var tanslateX = img1.ActualWidth * SIDE_DISTANCE_RATIO;

            double toValue = toNext ? -tanslateX : tanslateX;
            StoryboardHelper.CreatAnimation(img1.RenderTransform, storyboard, "(CompositeTransform.TranslateX)", duration, 0, toValue, ease, false);
            StoryboardHelper.CreatAnimation(img1.RenderTransform, storyboard, "(CompositeTransform.ScaleX)", duration, 0, SCALE_RATIO, ease, false);
            StoryboardHelper.CreatAnimation(img1.RenderTransform, storyboard, "(CompositeTransform.ScaleY)", duration, 0, SCALE_RATIO, ease, false);

            // img2
            StoryboardHelper.CreatAnimation(img2.RenderTransform, storyboard, "(CompositeTransform.TranslateX)", duration, beginTime, toValue, ease, false);
            StoryboardHelper.CreatAnimation(img2.RenderTransform, storyboard, "(CompositeTransform.ScaleX)", duration, 0, 1, ease, false);
            StoryboardHelper.CreatAnimation(img2.RenderTransform, storyboard, "(CompositeTransform.ScaleY)", duration, 0, 1, ease, false);

            // img3
            Canvas.SetLeft(img3, center - img3.ActualWidth / 2 + img3.ActualWidth * SIDE_DISTANCE_RATIO);
            var item = GetNextOrPreItem(img2, toNext);
            if (img3.DataContext != item)
            {
                img3.DataContext = item;
            }
            var trans = img3.RenderTransform as CompositeTransform;
            trans.ScaleX = SCALE_RATIO;
            trans.ScaleY = SCALE_RATIO;
            StoryboardHelper.CreatAnimation(img3, storyboard, "UIElement.Opacity", duration, 0, 1, null, false);

            // zindex
            ObjectAnimationUsingKeyFrames oa1 = new ObjectAnimationUsingKeyFrames();
            oa1.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
            Storyboard.SetTargetProperty(oa1,"(Canvas.ZIndex)");
            Storyboard.SetTarget(oa1,img1);

            ObjectAnimationUsingKeyFrames oa2 = new ObjectAnimationUsingKeyFrames();
            oa2.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 1 });
            Storyboard.SetTargetProperty(oa2, "(Canvas.ZIndex)");
            Storyboard.SetTarget(oa2, img2);

            storyboard.Children.Add(oa1);
            storyboard.Children.Add(oa2);


            storyboard.Begin();
            storyboard.Completed += (e1, e2) =>
            {
                Debug.WriteLine("Completed");
                UpdateTag(toNext);
                ResetPosition();
            };
        }

        private void ResetPosition()
        {
            foreach (var child in CanvasContainer.Children)
            {
                var trans = child.RenderTransform as CompositeTransform;
                InitPosition(child);
                trans.TranslateX = 0;
            }
        }

        /// <summary>
        /// 获取下个数据源
        /// </summary>
        private ImageItem GetNextOrPreItem(FrameworkElement element, bool toNext)
        {
            var index = _images.IndexOf(element.DataContext as ImageItem);
            if (index == -1) throw new Exception("index not found");
            index = toNext ? ++index : --index;
            if (index >= 0 || index < _images.Count)
            {
                return _images[index];
            }
            else
            {
                return null;
            }
        }

        private FrameworkElement GetItemByTag(int index)
        {
            var r = CanvasContainer.Children.Where(x => (x as FrameworkElement).Tag.ToString() == index.ToString());
            if (r.Count() > 1) throw new Exception("tag重复");
            return (FrameworkElement)r.FirstOrDefault();
        }

        private void UpdateTag(bool toNext)
        {
            foreach (var child in CanvasContainer.Children)
            {
                var tag = int.Parse((child as FrameworkElement).Tag.ToString());
                if (!toNext)
                {
                    (child as FrameworkElement).Tag = (++tag) % 4; // update tag
                }
                else
                {
                    (child as FrameworkElement).Tag = (--tag + 4) % 4;
                }
                Debug.WriteLine("tag " + (child as FrameworkElement).Tag);
            }
        }
        #endregion

        #region Interaction
        //-------------------------------------------------------------------
        private void Left_Tapped(object sender, RoutedEventArgs e)
        {
            // 只做了下一步
            return;
            GotoPreOrNext(false);
        }

        private void Right_Tapped(object sender, RoutedEventArgs e)
        {
            GotoPreOrNext(true);
        }
        #endregion

        private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("canvas size ");
        }
    }
}
