using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Menu.Child
{
    public sealed partial class CascadeMenuSubItem : CascadeMenuItemBase
    {
        #region fields
        // brushes
        private SolidColorBrush _hoverBackgroundBrush; // hover state
        private SolidColorBrush _hoverForegroundBrush;

        private SolidColorBrush _normalBackgroundBrush; // normal state
        private SolidColorBrush _normalForegroundBrush;

        private CascadeMenu _subMenu; // 子项菜单
        private Timer _delayTimer; // 延时显示计时器
        private bool _isHovering;
        #endregion

        #region ctor
        public CascadeMenuSubItem()
        {
            this.InitializeComponent();

            InitBrushes();
            Loaded += CascadeMenuSubItem_Loaded;
        }

        /// <summary>
        /// 初始化子菜单 after Loaded
        /// </summary>
        private void CascadeMenuSubItem_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= CascadeMenuSubItem_Loaded;

            InitItems();
        }
        #endregion

        #region property
        /// <summary>
        /// 子项
        /// </summary>
        public IList<CascadeMenuItemBase> Items { get; set; } = new List<CascadeMenuItemBase>();

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

        /// <summary>
        /// 关闭子菜单
        /// </summary>
        public override void CloseSubMenu()
        {
            _subMenu?.Close();
            OnUnhoverState();
        }

        // private

        #region state
        /// <summary>
        /// 显示子菜单
        /// </summary>
        private bool ShowSubMenu()
        {
            if (_subMenu == null) return false;

            if (!_subMenu.IsShowing) // 子菜单没有显示, 并且没有被关闭
            {
                _subMenu.ShowAt(this, new Point(ActualWidth - 20, ActualHeight - 20));
            }

            return _subMenu.IsShowing;
        }

        /// <summary>
        /// 设置focus状态
        /// </summary>
        private void OnHoverState()
        {
            _isHovering = true;
            Root.Background = _hoverBackgroundBrush;
            this.Foreground = _hoverForegroundBrush;
            ArrowElement.Foreground = _hoverForegroundBrush;
        }

        private void OnUnhoverState()
        {
            _isHovering = false;
            if (!_subMenu.IsShowing) // 子菜单没有显示 则 goto unhoverstate
            {
                Root.Background = _normalBackgroundBrush;
                this.Foreground = _normalForegroundBrush;
                ArrowElement.Foreground = _normalForegroundBrush;
            }
        }
        #endregion

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
            if (Items == null || Items.Count <= 0 || _subMenu != null) return; // 没有数据源 或 已初始化

            _subMenu = new CascadeMenu(false)
            {
                Items = Items,
            };
            _subMenu.SelectionChanged += _subMenu_SelectionChanged;
        }

        /// <summary>
        /// invoke 子项选择事件
        /// </summary>
        private void _subMenu_SelectionChanged(CascadeMenuItemBase arg1, object arg2)
        {
            base.OnSelectionChanged(arg1, arg2);
        }
        #endregion

        #region event

        #region 指针进入/离开
        private void OnPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            StartTimer();
            OnHoverState();
            EnteredAction?.Invoke(this, true);

            Debug.WriteLine("进入subItem");
        }

        private void OnPointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OnUnhoverState();
            if (!_subMenu.IsShowing) // 指针离开 && 没有显示子项
            {
                EnteredAction?.Invoke(this, false);
            }

            Debug.WriteLine("离开subItem");
        }

        /// <summary>
        /// 延时显示子菜单
        /// </summary>
        private void StartTimer()
        {
            if (_delayTimer == null)
            { _delayTimer = new Timer(TimerCallBack, null, 300, -1); }
            else
            { _delayTimer.Change(300, -1); }
        }

        /// <summary>
        /// 延时显示子菜单
        /// </summary>
        private void TimerCallBack(object state)
        {
            Debug.WriteLine("TimerCallBack " + _isHovering);
            if (_isHovering)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                {
                    ShowSubMenu(); // 显示子菜单
                });
            }
        }

        #endregion

        #endregion
    }
}
