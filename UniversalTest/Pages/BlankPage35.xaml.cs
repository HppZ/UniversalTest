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
    public sealed partial class BlankPage35 : Page, IIncrementalSource<string>
    {
        public BlankPage35()
        {
            this.InitializeComponent();
            xamlListView.ItemsSource = new IncrementalLoadingObservableCollection<string>(this);
        }

        public bool HasMoreItems { get; set; } = true;
        public async Task<IEnumerable<string>> GetItems(CancellationToken token, uint count)
        {
            await Task.Delay(1);
            return Enumerable.Range(0, (int)count).Select(x => x.ToString());
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.HasMoreItems = !this.HasMoreItems;
        }
    }
}
