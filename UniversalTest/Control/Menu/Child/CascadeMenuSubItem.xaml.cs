using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Menu.Child
{
    public sealed partial class CascadeMenuSubItem : UserControl
    {
        #region fields
        // brushes
        private SolidColorBrush _hoverBackgroundBrush; // hover state
        private SolidColorBrush _hoverForegroundBrush;

        private SolidColorBrush _normalBackgroundBrush; // normal state
        private SolidColorBrush _normalForegroundBrush;
        #endregion

        public CascadeMenuSubItem()
        {
            this.InitializeComponent();
            Loaded += CascadeMenuSubItem_Loaded;
            InitBrushes();
        }

        private void CascadeMenuSubItem_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            InitItems();
        }

        #region property
        /// <summary>
        /// 子项
        /// </summary>
        public IList<Windows.UI.Xaml.Controls.Control> Items { get; set; } = new List<Windows.UI.Xaml.Controls.Control>();

        /// <summary>
        /// 显示图标
        /// </summary>
        public ImageSource Icon { get; set; }

        /// <summary>
        /// 显示文字
        /// </summary>
        public string Text { get; set; }
        #endregion



        // public



        // private

        #region init
        /// <summary>
        /// 初始化画刷
        /// </summary>
        private void InitBrushes()
        {
            _hoverBackgroundBrush = Resources["HoverBackgroundBrush"] as SolidColorBrush;
            _hoverForegroundBrush = Resources["HoverForegroundBrush"] as SolidColorBrush;

            _normalBackgroundBrush = Resources["NormalBackgroundBrush"] as SolidColorBrush;
            _normalForegroundBrush = Resources["NormalForegroundBrush"] as SolidColorBrush;
        }

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
        #endregion

        #region state
        /// <summary>
        /// 处理指针hover状
        /// </summary>
        private void OnHover(bool toHover)
        {
            if (toHover)
            {
                Root.Background = _hoverBackgroundBrush;
                this.Foreground = _hoverForegroundBrush;
                ArrowElement.Foreground = _hoverForegroundBrush;
            }
            else
            {
                Root.Background = _normalBackgroundBrush;
                this.Foreground = _normalForegroundBrush;
                ArrowElement.Foreground = _normalForegroundBrush;
            }
        }
        #endregion

        #region 指针事件
        private void OnPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OnHover(true);
        }

        private void OnPointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OnHover(false);
        }
        #endregion


    }
}
