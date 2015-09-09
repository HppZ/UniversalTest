using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Sight.Windows10.Helper;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sight.Windows10.Views.Controls
{
    public sealed partial class ExpandableControl : UserControl
    {
        #region 变量
        private bool isExpanded = false;
        #endregion

        public ExpandableControl()
        {
            this.InitializeComponent();
        }

        #region 向外抛出的event
        public event TappedEventHandler UserActionTapped;
        #endregion

        #region 属性

        public static readonly DependencyProperty TitleInfoTextProperty = DependencyProperty.Register(
            "TitleInfoText", typeof (string), typeof (ExpandableControl), new PropertyMetadata(default(string)));

        public string TitleInfoText
        {
            get { return (string) GetValue(TitleInfoTextProperty); }
            set { SetValue(TitleInfoTextProperty, value); }
        }

        public string Title { get; set; }
        public double ControlHeight { get; set; }
        public UIElement UserActionContent { get; set; }
        public Visibility UserActionVisibility { get; set; }
        public DataTemplate ItemTemplate { get; set; }

        #endregion

        #region Tapped
        /// <summary>
        /// 动画
        /// </summary>
        private void Animation()
        {
            var scrollview = VisualTreeExtensions.FindFirstElementInVisualTree<ScrollViewer>(GroupItemListView);
            var rotate = RotateImg.RenderTransform as RotateTransform;

            DoubleAnimation da = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(200),
                EnableDependentAnimation = true,
                EasingFunction = new PowerEase()
                {
                    EasingMode = EasingMode.EaseIn
                },
            };

            if (isExpanded)
            {
                isExpanded = false;
                var endvalue = UserActionVisibility == Visibility.Visible
                    ? (GroupItemListView.ActualHeight + 44)
                    : GroupItemListView.ActualHeight;
                rotate.Angle = 0;
                da.To = 0;
                da.From = endvalue;
            }
            else
            {
                isExpanded = true;
                var endvalue = UserActionVisibility == Visibility.Visible
                    ? (scrollview.ExtentHeight + 44)
                    : scrollview.ExtentHeight;
                rotate.Angle = 45;
                da.To = endvalue;
                da.From = 0;
            }
            Storyboard.SetTarget(da, ExpandableStp);
            Storyboard.SetTargetProperty(da, "Height");
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
        }

      
        /// <summary>
        /// 点击控件展开/收起
        /// </summary>
        private void GroupStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Animation();
        }

        #endregion

        #region Event
        private void UserAction_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            UserActionTapped?.Invoke(sender, e);
        }

        #endregion

        public void UndoSelected()
        {
            GroupItemListView.SelectedItem = null;
        }
    }
}
