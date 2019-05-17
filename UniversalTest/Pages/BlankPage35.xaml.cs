using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage35 : Page, IIncrementalSource<ItemModel>
    {
        private BindingObject _bindingObj;
        public BlankPage35()
        {
            this.InitializeComponent();
            _bindingObj = new BindingObject()
            {
                ItemMargin = new Thickness(0, 0, 20, 0)
            };

            ListViewElement.ItemsSource = new List<IncrementalLoadingObservableCollection<ItemModel>>()
            {
                new IncrementalLoadingObservableCollection<ItemModel>(this),
            };
        }

        public bool HasMoreItems { get; set; } = true;
        public async Task<IEnumerable<ItemModel>> GetItems(CancellationToken token, uint count)
        {
            var list = new List<ItemModel>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new ItemModel()
                {
                    Text = i.ToString(),
                    BindingObject = _bindingObj
                });
            }
            return await Task.FromResult(list);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.HasMoreItems = !this.HasMoreItems;
        }
    }

    public class BindingObject : DependencyObject
    {
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            "ItemMargin", typeof(Thickness), typeof(BindingObject), new PropertyMetadata(default(Thickness)));

        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
    }

    public class ItemModel
    {
        public string Text { get; set; }
        public BindingObject BindingObject { get; set; }
    }

    class ListView1 : ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var uiElment = element as FrameworkElement;
            var model = (item as ItemModel);
            uiElment.DataContext = model;

            BindingOperations.SetBinding(
                uiElment,
                MarginProperty,
                new Binding { Path = new PropertyPath("BindingObject.ItemMargin") });

            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            var uiElment = element as FrameworkElement;
            uiElment.ClearValue(MarginProperty);

            base.ClearContainerForItemOverride(element, item);
        }
    }

}
