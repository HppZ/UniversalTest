using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using UniversalTest.Control.ScrollViewer.Children;
using UniversalTest.Controller;
using UniversalTest.Helper;
using ViewerItem = UniversalTest.Control.ScrollViewer.Second.Children.ViewerItem;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.ScrollViewer.Second
{
    public sealed partial class ThreeImageViewer : UserControl
    {
        #region Fields
        private List<ImageItem> _images; // 数据源
        private ImageItem _curItem; // 当前item
        private const double IMAGE_WH_RATIO = 4.0 / 3; // 照片宽高比
        private const double SCALE_RATIO = 0.80; // 缩放比例
        private const double SIDE_DISTANCE_RATIO = 16.0 / 16; // 两边间距占Image的比例
        private bool _inited; // 是否初始化过
        private bool _isStoryboardCompleted; // 动画是否结束
        private bool _loaded; // 是否加载完成
        private Storyboard _storyboard; // 当前动画


        private MainController _mainController;
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
            _loaded = true;
            if (!_inited)
            {
                Init();
            }
        }
        #endregion

        #region Init
        /// <summary>
        /// 初始化
        /// </summary>
        private async void Init()
        {
            if (_inited) return;
            _inited = true;

            _mainController = new MainController();
            await _mainController.Init();
            _images = _mainController.Source.ToList();

            var start = _images.IndexOf(_curItem) - 1;
            for (int i = 0; i < CanvasContainer.Children.Count; i++)
            {
                var child = CanvasContainer.Children[i];
                child.RenderTransform = new CompositeTransform();
                child.RenderTransformOrigin = new Point(0.5, 0.5);
                var index = start + i;
                if (index >= 0 && index < _images.Count)
                {
                    (child as FrameworkElement).DataContext = _images[start + i];
                }
                InitPosition(child);
            }
        }

        /// <summary>
        /// 更新子项位置
        /// </summary>
        private void InitPosition(UIElement element)
        {
            var center = CanvasContainer.ActualWidth / 2;
            var item = element as ViewerItem;
            var trans = item.RenderTransform as CompositeTransform;
            if (item.Tag.ToString() == "0")
            {
                var v = center - item.ActualWidth / 2 - item.ActualWidth * SIDE_DISTANCE_RATIO;
                Canvas.SetLeft(element, v);
                Canvas.SetZIndex(element, 0);
                trans.ScaleX = SCALE_RATIO;
                trans.ScaleY = SCALE_RATIO;
                item.ShowMask(true, true);
                item.Opacity = 1;
            }
            else if (item.Tag.ToString() == "1")
            {
                var v = center - item.ActualWidth / 2;
                Canvas.SetLeft(element, v);
                Canvas.SetZIndex(item, 1);
                item.ShowMask(false, true);
                item.Opacity = 1;
            }
            else if (item.Tag.ToString() == "2")
            {
                var v = center - item.ActualWidth / 2 + item.ActualWidth * SIDE_DISTANCE_RATIO;
                Canvas.SetLeft(element, v);
                Canvas.SetZIndex(element, 0);
                trans.ScaleX = SCALE_RATIO;
                trans.ScaleY = SCALE_RATIO;
                item.ShowMask(true, true);
                item.Opacity = 1;
            }
            else if (item.Tag.ToString() == "3")
            {
                item.Opacity = 0;
            }
        }
        #endregion

        #region Dep property
        /// <summary>
        /// 选择项index
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof(int), typeof(ThreeImageViewer), new PropertyMetadata(default(int), OnSelectedIndexChanged));
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThreeImageViewer viewer = d as ThreeImageViewer;
            if (viewer._inited)
            {
                viewer.SelectionChanged((int)e.NewValue, (int)e.OldValue);
            }
        }
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        #endregion

        #region Auto Layout
        /// <summary>
        /// 大小改变
        /// </summary>
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
            if (_storyboard != null && !_isStoryboardCompleted)
            {
                _storyboard?.Stop();
                ResetPosition();
            }

            _isStoryboardCompleted = false;
            var img0 = GetItemByTag(0);
            var img1 = GetItemByTag(1);
            var img2 = GetItemByTag(2);
            var img3 = GetItemByTag(3);

            var center = CanvasContainer.ActualWidth / 2;
            var duration = 600;
            var beginTime = 60;
            _storyboard = new Storyboard();
            var ease = new QuarticEase() { EasingMode = EasingMode.EaseOut };

            ViewerItem target0, target2;
            if (toNext)
            {
                target0 = img0;
                target2 = img2;
            }
            else
            {
                target0 = img2;
                target2 = img0;
            }

            // img0
            StoryboardHelper.CreatAnimation(target0, _storyboard, "UIElement.Opacity", 110, 0, 0, null, false);

            // img1
            var tanslateX = img1.ActualWidth * SIDE_DISTANCE_RATIO;

            double toValue = toNext ? -tanslateX : tanslateX;
            StoryboardHelper.CreatAnimation(img1.RenderTransform, _storyboard, "(CompositeTransform.TranslateX)", duration, 0, toValue, ease, false);
            StoryboardHelper.CreatAnimation(img1.RenderTransform, _storyboard, "(CompositeTransform.ScaleX)", duration, 0, SCALE_RATIO, ease, false);
            StoryboardHelper.CreatAnimation(img1.RenderTransform, _storyboard, "(CompositeTransform.ScaleY)", duration, 0, SCALE_RATIO, ease, false);

            // img2
            StoryboardHelper.CreatAnimation(target2.RenderTransform, _storyboard, "(CompositeTransform.TranslateX)", duration, beginTime, toValue, ease, false);
            StoryboardHelper.CreatAnimation(target2.RenderTransform, _storyboard, "(CompositeTransform.ScaleX)", duration, 0, 1, ease, false);
            StoryboardHelper.CreatAnimation(target2.RenderTransform, _storyboard, "(CompositeTransform.ScaleY)", duration, 0, 1, ease, false);

            // img3
            Canvas.SetLeft(img3, center - img3.ActualWidth / 2 + img3.ActualWidth * SIDE_DISTANCE_RATIO);
            var item = GetNextOrPreItem(target2, toNext);
            if (img3.DataContext != item)
            {
                img3.DataContext = item;
            }
            Canvas.SetLeft(img3, Canvas.GetLeft(target2));
            var trans = img3.RenderTransform as CompositeTransform;
            trans.ScaleX = SCALE_RATIO;
            trans.ScaleY = SCALE_RATIO;
            StoryboardHelper.CreatAnimation(img3, _storyboard, "UIElement.Opacity", duration, 0, 1, null, false);

            // zindex
            ObjectAnimationUsingKeyFrames oa1 = new ObjectAnimationUsingKeyFrames() {BeginTime = TimeSpan.FromMilliseconds(0)};
            oa1.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
            Storyboard.SetTargetProperty(oa1, "(Canvas.ZIndex)");
            Storyboard.SetTarget(oa1, img1);

            ObjectAnimationUsingKeyFrames oa2 = new ObjectAnimationUsingKeyFrames() {BeginTime = TimeSpan.FromMilliseconds(0)};
            oa2.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 1 });
            Storyboard.SetTargetProperty(oa2, "(Canvas.ZIndex)");
            Storyboard.SetTarget(oa2, target2);

            _storyboard.Children.Add(oa1);
            _storyboard.Children.Add(oa2);

            target0.ShowMask(true);
            target2.ShowMask(false);
            img1.ShowMask(true);
            img3.ShowMask(true);

            UpdateTag(toNext);
            _storyboard.Begin();
            _storyboard.Completed += (e1, e2) =>
            {
                _isStoryboardCompleted = true;
                ResetPosition();
            };

        }

        /// <summary>
        /// 重置item的位置
        /// </summary>
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
            index = toNext ? ++index : --index;
            if (index >= 0 && index < _images.Count)
            {
                return _images[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过tag找到item
        /// </summary>
        private ViewerItem GetItemByTag(int index)
        {
            var r = CanvasContainer.Children.Where(x => (x as FrameworkElement).Tag.ToString() == index.ToString());
            return (ViewerItem)r.FirstOrDefault();
        }

        /// <summary>
        /// 更新tag
        /// </summary>
        /// <param name="toNext"></param>
        private void UpdateTag(bool toNext)
        {
            foreach (var child in CanvasContainer.Children)
            {
                var tag = int.Parse((child as FrameworkElement).Tag.ToString());
                if (!toNext)
                {
                    (child as FrameworkElement).Tag = (++tag) % 4;
                }
                else
                {
                    (child as FrameworkElement).Tag = (--tag + 4) % 4;
                }
            }
        }

        /// <summary>
        /// 当前项变化
        /// </summary>
        private void SelectionChanged(int newIndex, int oldIndex)
        {
            var old = _images.IndexOf(_curItem);
            if (old == newIndex || newIndex == -1) return;
            int start = -1, end = -1;
            if (old > newIndex)
            {
                start = newIndex;
                end = old;
            }
            else
            {
                start = old;
                end = newIndex;
            }

            for (int i = start; i < end; i++)
            {
                GotoPreOrNext(newIndex > old);
            }
            _curItem = _images[newIndex];
        }
        #endregion

        #region Public
        /// <summary>
        /// 设置数据源
        /// </summary>
        public void SetSource(List<ImageItem> list)
        {
            _images = list;
        }

        /// <summary>
        ///  设置当前项
        /// </summary>
        public void SetCurItem(ImageItem curItem)
        {
            _curItem = curItem;
            if (!_inited && _loaded)
            {
                Init();
            }
        }

        #endregion

    }
}
