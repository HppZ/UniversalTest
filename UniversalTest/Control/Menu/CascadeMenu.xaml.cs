using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Menu
{
    public sealed partial class CascadeMenu : UserControl
    {
        public CascadeMenu()
        {
            this.InitializeComponent();
            Loaded += CascadeMenu_Loaded;
        }

        private void CascadeMenu_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            InitItems();
        }


        #region property

        /// <summary>
        /// 所有子项
        /// </summary>
        public IList<Windows.UI.Xaml.Controls.Control> Items { get; set; } = new List<Windows.UI.Xaml.Controls.Control>();
        #endregion

        // public




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
