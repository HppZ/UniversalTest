using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UniversalTest.Control.Menu;
using UniversalTest.Control.Menu.Child;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage3 : Page
    {
        ObservableCollection<MyClass> _vm = new ObservableCollection<MyClass>();

        private CascadeMenu _cascadeMenu;
        /// <summary>
        /// 右键菜单 popup
        /// </summary>
        public BlankPage3()
        {
            this.InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                _vm.Add(new MyClass() {Text = i.ToString()});
            }
            Loaded += BlankPage3_Loaded;
           
        }

        private void BlankPage3_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= BlankPage3_Loaded;

            _cascadeMenu = new CascadeMenu();
            _cascadeMenu.Items.Add(new CascadeMenuItem() {Text = "123"});
            _cascadeMenu.Items.Add(new CascadeMenuSeparator());
            _cascadeMenu.Items.Add(new CascadeMenuListViewItem() { ItemsSource = _vm, MaxHeight = 200 });
            _cascadeMenu.Items.Add(new CascadeMenuSeparator());
            _cascadeMenu.Items.Add(new CascadeMenuSubItem()
            {
                Text = "abc",
                Items =
                {
                    new CascadeMenuItem() { Text = "456"},
                    new CascadeMenuSeparator(),
                    new CascadeMenuListViewItem() {ItemsSource = _vm, MaxHeight = 200}
                }
            });

            _cascadeMenu.SelectionChanged += _cascadeMenu_SelectionChanged;

        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        private void _cascadeMenu_SelectionChanged(CascadeMenuItemBase arg1, object arg2)
        {
            Debug.WriteLine(arg1);
            Debug.WriteLine(arg2);
        }

        /// <summary>
        /// 显示菜单
        /// </summary>
        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            _cascadeMenu.ShowAt(sender as UIElement, new Point(0, 50));
        }
    }

    class MyClass
    {
        public ImageSource Icon { get; set; }
        public string Text { get; set; }
    }
}
