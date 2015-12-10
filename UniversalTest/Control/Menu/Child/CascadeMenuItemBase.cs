using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UniversalTest.Control.Menu.Child
{
    public class CascadeMenuItemBase : UserControl
    {
        #region cotr
        public CascadeMenuItemBase()
        {
            Height = 40;
        }
        #endregion

        #region action
        /// <summary>
        /// 是否处于焦点
        /// </summary>
        public Action<CascadeMenuItemBase, bool> EnteredAction { get; set; }

        /// <summary>
        /// 点击item选择改变
        /// </summary>
        public event Action<CascadeMenuItemBase, object> SelectionChanged;
        #endregion

        #region virtual
        /// <summary>
        /// 关闭子菜单
        /// </summary>
        public virtual void CloseSubMenu()
        {
            // ignored
        }

        /// <summary>
        /// invoke 选择改变事件
        /// </summary>
        protected void OnSelectionChanged(CascadeMenuItemBase item, object param)
        {
            SelectionChanged?.Invoke(item, param);
        }

        #endregion
    }
}
