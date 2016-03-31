using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace UniversalTest.Control.Search
{
    public sealed partial class SearchTextBox : TextBox
    {
        #region fields
        private SearchPanel _searchPanel;
        #endregion

        public SearchTextBox()
        {
            this.InitializeComponent();
            this.Style = this.Resources["TextBoxStyle1"] as Style;
            ContextMenuOpening += SearchTextBox_ContextMenuOpening;
            this.TextChanged += SearchTextBox_TextChanged;
        }


        #region private methods

        private void ShowSearchResult()
        {
            if (_searchPanel == null)
            {
                _searchPanel = new SearchPanel();
            }

            // 确定显示位置
            var point = this.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
            var pos = new Point(point.X, point.Y + this.ActualHeight + 10);
            _searchPanel.Show(pos, string.IsNullOrEmpty(Text));
        }

        private void CloseSearchResult()
        {
            _searchPanel?.Close();
        }

        #endregion


        #region override

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            ShowSearchResult();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            CloseSearchResult();
            base.OnLostFocus(e);
        }

        #endregion



        #region private events
        private void SearchTextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                _searchPanel?.Swith();
            }
        }

        #endregion
    }
}
