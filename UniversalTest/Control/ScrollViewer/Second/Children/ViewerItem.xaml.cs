using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using UniversalTest.Helper;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.ScrollViewer.Second.Children
{
    public sealed partial class ViewerItem : UserControl
    {
        private const double MAX_OPACITY = 0.5; // 遮罩最大透明度
        public ViewerItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示黑色遮罩
        /// </summary>
        public void ShowMask(bool toShow, bool disableAnimation = false)
        {
            if (disableAnimation)
            {
                Mask.Opacity = toShow ? MAX_OPACITY : 0;
            }
            else
            {
                Storyboard storyboard = new Storyboard();
                StoryboardHelper.CreatAnimation(Mask, storyboard, "UIElement.Opacity", 200, 0, toShow ? MAX_OPACITY : 0, null, false);
                storyboard.Begin();
            }
        }
    }
}
