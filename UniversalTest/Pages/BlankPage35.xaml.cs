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

            ListViewElement.ItemsSource = new List<IncrementalLoadingObservableCollection<string>>()
            {
                new IncrementalLoadingObservableCollection<string>(this),
            };
        }

        public bool HasMoreItems { get; set; } = true;

        public async Task<IEnumerable<string>> GetItems(CancellationToken token, uint count)
        {
            var list = new List<string>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i.ToString());
            }
            return await Task.FromResult(list);
        }
    }
}
