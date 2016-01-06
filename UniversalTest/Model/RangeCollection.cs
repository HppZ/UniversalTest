using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.BulkAccess;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using UniversalTest.Controller;

namespace UniversalTest.Model
{
    /// <summary>
    /// impl IItemsRangeInfo
    /// </summary>
    public class RangeCollection : ObservableCollection<ImageItem>, IItemsRangeInfo
    {
        private List<Request> _requesting = new List<Request>();
        //private FileInformationFactory _factory;
        private StorageFileQueryResult _fileQueryResult;
        public RangeCollection(IEnumerable<ImageItem> collection) : base(collection)
        {
            Init();
        }

        public void Dispose()
        {

        }

        public void Init()
        {
            QueryOptions options = new QueryOptions(CommonFileQuery.OrderBySearchRank, new List<string>() { ".jpg", ".png" });
            _fileQueryResult = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(options);
            //_factory = new FileInformationFactory(query, ThumbnailMode.SingleItem, 256, ThumbnailOptions.UseCurrentScale, true);
        }


        /// <summary>
        /// 显示范围改变
        /// </summary>
        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            var r = (from x in _requesting
                     where
                         x.IndexRange.FirstIndex == visibleRange.FirstIndex
                         &&
                         x.IndexRange.LastIndex == visibleRange.LastIndex

                     select x).ToList();
            if (r.Any()) return;
            Debug.WriteLine("first " + visibleRange.FirstIndex + "  last " + visibleRange.LastIndex);

            // 检查已经不需要的请求
            CancelNotNeededRequest(visibleRange);

            // 新请求
            CancellationTokenSource source = new CancellationTokenSource();
            Request request = new Request(source, visibleRange);
            _requesting.Add(request); // 添加新请求
            try
            {
                GetDataRequest(request.IndexRange, request.Cancellation.Token);
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {
            }
        }

        private async void GetDataRequest(ItemIndexRange range, CancellationToken ct)
        {
            try
            {
                var results = await _fileQueryResult.GetFilesAsync((uint)range.FirstIndex, Math.Max(range.Length, 30)).AsTask(ct);
                if (results != null)
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        ct.ThrowIfCancellationRequested();
                        var t = Items.First(x => x.LocalPath == results[i].Path);
                        if (!t.Loaded)
                        {
                           await t.SetPreviewImage(ct);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
            }
        }


        /// <summary>
        /// 取消不需要的请求
        /// </summary>
        public void CancelNotNeededRequest(ItemIndexRange range)
        {
            var r = (from x in _requesting
                     where x.IndexRange.FirstIndex - 20 > range.LastIndex || x.IndexRange.LastIndex +20 < range.FirstIndex
                     select x).ToList();
            foreach (var request in r)
            {
                _requesting.Remove(request); // 移出改请求
                request.Cancel();
            }
        }


    }

    class Request
    {
        public Request(CancellationTokenSource s, ItemIndexRange i)
        {
            Cancellation = s;
            IndexRange = i;
        }
        public CancellationTokenSource Cancellation { get; set; }
        public ItemIndexRange IndexRange { get; set; }

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            Cancellation?.Cancel();
        }
    }

}
