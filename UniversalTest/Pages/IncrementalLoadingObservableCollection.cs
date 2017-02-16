using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace iQiyiVideo.Common
{
    public class IncrementalLoadingObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private bool _busy = false;

        public Func<bool> HasMoreItemsFunc;
        public Func<CancellationToken, uint, Task<IList<T>>> LoadMoreItemsAsyncFunc;

        public bool HasMoreItems
        {
            get
            {
                if (HasMoreItemsFunc == null)
                {
                    return false;
                }

                return HasMoreItemsFunc.Invoke();
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_busy)
            {
                throw new InvalidOperationException("重复加载");
            }

            _busy = true;
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                var items = await LoadMoreItemsAsyncFunc(c, count);
                foreach (T item in items)
                {
                    this.Add(item);
                }
                return new LoadMoreItemsResult { Count = (uint)items.Count };
            }
            finally
            {
                _busy = false;
            }
        }

    }
}
