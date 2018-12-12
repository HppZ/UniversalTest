using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace iQiyiVideo.Common
{
    public interface IIncrementalSource<T>
    {
        bool HasMoreItems { get; }
        Task<IEnumerable<T>> GetItems(CancellationToken token, uint count);
    }

    public class IncrementalLoadingObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private readonly IIncrementalSource<T> _incrementalSource;

        public IncrementalLoadingObservableCollection(IIncrementalSource<T> source)
        {
            _incrementalSource = source;
        }

        public bool HasMoreItems => _incrementalSource.HasMoreItems;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            Debug.WriteLine($"LoadMoreItemsAsync {count}");
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            var items = await _incrementalSource.GetItems(c, count);
            foreach (T item in items)
            {
                this.Add(item);
            }
            return new LoadMoreItemsResult { Count = (uint)items.Count() };
        }

    }
}
