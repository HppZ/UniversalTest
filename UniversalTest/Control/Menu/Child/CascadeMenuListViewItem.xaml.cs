using System;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Menu.Child
{
    public sealed partial class CascadeMenuListViewItem : CascadeMenuItemBase
    {

        #region ctor
        public CascadeMenuListViewItem()
        {
            this.InitializeComponent();
            Height = double.NaN;
        }
        #endregion

        #region property
        /// <summary>
        /// 改变默认数据模板
        /// </summary>
        public DataTemplate ItemDataTemplate
        {
            set
            {
                if (value != null)
                {
                    RootListView.ItemTemplate = value;
                }
            }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof (object), typeof (CascadeMenuListViewItem), new PropertyMetadata(default(object)));

        public object ItemsSource
        {
            get { return (object) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion


        #region events
        /// <summary>
        /// 选择改变
        /// </summary>
        private void OnItemTapped(object sender, TappedRoutedEventArgs e)
        {
            base.OnSelectionChanged(this, sender);
        }
        #endregion


        #region 指针 进入/ 离开
        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            EnteredAction?.Invoke(this, true);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            EnteredAction?.Invoke(this, false);
        }

        private void OnItemPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }
        #endregion


    }
}
