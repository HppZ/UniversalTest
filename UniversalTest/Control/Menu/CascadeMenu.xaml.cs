using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using UniversalTest.Control.Menu.Child;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Menu
{
    public sealed partial class CascadeMenu : UserControl
    {
        #region fields

        private Popup _popup; // 弹出菜单
        private readonly bool _isLightDismissEnabled; // 自动消失 默认为true
        CascadeMenuItemBase _hoverItem;
        #endregion

        #region ctor
        public CascadeMenu(bool lightDismiss = true)
        {
            this.InitializeComponent();

            _isLightDismissEnabled = lightDismiss;
            Init();
            Loaded += CascadeMenu_Loaded;
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void Init()
        {
            if (_popup == null)
            {
                _popup = new Popup()
                {
                    Child = this,
                };
                _popup.Closed += _popup_Closed;
                _popup.Opened += _popup_Opened;
            }
        }

        /// <summary>
        /// 加载一次， 初始化一次
        /// </summary>
        private void CascadeMenu_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= CascadeMenu_Loaded;
            InitItems();
        }
        #endregion

        #region property

        /// <summary>
        /// 所有子项
        /// </summary>
        public IList<CascadeMenuItemBase> Items { get; set; } = new List<CascadeMenuItemBase>();

        /// <summary>
        /// 是否正在显示
        /// </summary>
        public bool IsShowing => _popup != null && _popup.IsOpen;

        /// <summary>
        /// 点击菜单某项
        /// </summary>
        public event Action<CascadeMenuItemBase, object> SelectionChanged; 

        #endregion

        #region action

        public event Action<CascadeMenu, bool> MenuOpenedEvent;// 菜单 显示/隐藏通知
        #endregion


        // public

        #region 显示/隐藏 菜单
        /// <summary>
        /// 显示菜单
        /// </summary>
        public void ShowAt(UIElement element, Point point)
        {
            if (element == null) return;

            var p = element.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
            _popup.IsLightDismissEnabled = _isLightDismissEnabled;
            _popup.HorizontalOffset = p.X + point.X;
            _popup.VerticalOffset = p.Y + point.Y;
            _popup.IsOpen = true;
        }

        /// <summary>
        /// 关闭菜单
        /// </summary>
        public void Close()
        {
            _popup.IsOpen = false;
        }
        #endregion


        // private
        #region 初始化子项
        /// <summary>
        /// 添加子项
        /// </summary>
        private void InitItems()
        {
            if (Items == null) return;

            foreach (var item in Items)
            {
                item.EnteredAction = OnPointerEnteredItem;
                item.SelectionChanged += Item_SelectionChanged;
                ChildrenContainer.Children.Add(item);
            }
        }

       

        #endregion

        #region 子项
        /// <summary>
        /// 指针进入子项或者离开
        /// </summary>
        private void OnPointerEnteredItem(CascadeMenuItemBase item, bool isEnterd)
        {
            if (_hoverItem == item) return;

            if (isEnterd) // 指针进入某项
            {
                _hoverItem?.CloseSubMenu(); // 关闭原来的项的子菜单(if exist)
                _hoverItem = item; // 记录新项
            }

        }


        /// <summary>
        /// 关闭所有子项的菜单
        /// </summary>
        private void CloseChildren()
        {
            foreach (var item in Items)
            {
                CloseItemSubMenu(item);
            }
        }

        /// <summary>
        /// 关闭某个item的子项菜单
        /// </summary>
        private void CloseItemSubMenu(CascadeMenuItemBase item)
        {
            item.CloseSubMenu();
        }

        /// <summary>
        /// 子项选择改变
        /// </summary>
        private void Item_SelectionChanged(CascadeMenuItemBase arg1, object arg2)
        {
            // invoke & close menu
            SelectionChanged?.Invoke(arg1, arg2);
            Close();
        }
        #endregion

        #region 菜单显示/隐藏 event
        /// <summary>
        /// 菜单显示事件
        /// </summary>
        private void _popup_Opened(object sender, object e)
        {
            MenuOpenedEvent?.Invoke(this, true);
        }

        /// <summary>
        /// 菜单消失事件
        /// </summary>
        private void _popup_Closed(object sender, object e)
        {
            CloseChildren();
            MenuOpenedEvent?.Invoke(this, false);
        }

        #endregion
    }
}
