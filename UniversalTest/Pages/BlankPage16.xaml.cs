using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using iQiyiVideo.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage16 : Page
    {
        public IncrementalLoadingObservableCollection<string> SearchResultObservableCards;
        private int start = 1;
        public BlankPage16()
        {
            this.InitializeComponent();
            SearchResultObservableCards = new IncrementalLoadingObservableCollection<string>();
            listViewSearch.ItemsSource = SearchResultObservableCards;
        }

        private void load()
        {
            for (int i = (start - 1) * 100; i < start * 100; i++)
            {
                SearchResultObservableCards.Add(i.ToString());
            }
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            load();
        }

        private void UIElement_OnTapped2(object sender, TappedRoutedEventArgs e)
        {
            SearchResultObservableCards.Clear();
        }

        private void ListViewSearch_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {

        }

        private void ListViewSearch_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {

        }
    }


    #region 相关类

    interface ILoadElements
    {

    }


    #endregion


}
