using System.Collections;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Menu
{
    public sealed partial class CascadeMenu : UserControl
    {
        #region fields

        private Popup _popup; // 弹出菜单
        #endregion

        #region ctor
        public CascadeMenu()
        {
            this.InitializeComponent();
            Loaded += CascadeMenu_Loaded;
        }
        #endregion

        private void CascadeMenu_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            InitItems();
        }


        #region property

        /// <summary>
        /// 所有子项
        /// </summary>
        public IList<Windows.UI.Xaml.Controls.Control> Items { get; set; } = new List<Windows.UI.Xaml.Controls.Control>();

        /// <summary>
        /// 是否正在显示
        /// </summary>
        public bool IsShowing => _popup.IsOpen;

        #endregion

        // public

        /// <summary>
        /// 显示菜单
        /// </summary>
        public void ShowAt(UIElement element, Point point)
        {
            if (element == null) return;

            var p = element.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));

            if (_popup == null)
            {
                _popup = new Popup()
                {
                    IsLightDismissEnabled = true,
                    Child = this,
                };
                _popup.Closed += _popup_Closed;
            }

            _popup.HorizontalOffset = p.X + point.X;
            _popup.VerticalOffset = p.Y + point.Y;
            _popup.IsOpen = true;
        }

        private void _popup_Closed(object sender, object e)
        {

        }

        /// <summary>
        /// 关闭菜单
        /// </summary>
        public void Close()
        {
            _popup.IsOpen = false;
        }
        // private

        /// <summary>
        /// 添加子项
        /// </summary>
        private void InitItems()
        {
            if (Items == null) return;

            foreach (var item in Items)
            {
                ChildrenContainer.Children.Add(item);
            }
        }
    }
}
