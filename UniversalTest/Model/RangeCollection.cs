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
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using UniversalTest.Controller;
using UniversalTest.Helper;

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
        private ItemIndexRange _currentRange;
        bool _allLoaded;
        public RangeCollection(IEnumerable<ImageItem> list) : base(list)
        {

        }

        public void Dispose()
        {
            Debug.WriteLine("Dispose ##############");
        }

        public void Init()
        {
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByDate,
                new string[] { ".jpg", ".png", ".jpeg", ".bmp" })
            {
                FolderDepth = FolderDepth.Deep,
                IndexerOption = IndexerOption.OnlyUseIndexer,
                UserSearchFilter = "System.Kind:=System.Kind#Picture"
            };
            queryOptions.SetThumbnailPrefetch(ThumbnailMode.SingleItem, 256, ThumbnailOptions.UseCurrentScale);
            _fileQueryResult = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(queryOptions);

            GetAll();
        }

        private async void GetAll()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            await Task.Factory.StartNew(async () =>
           {
               foreach (var item in Items)
               {
                   await item.SetPreviewImage(source.Token);
               }
               _allLoaded = true;
           }, TaskCreationOptions.LongRunning);
            //await ThreadPool.RunAsync(async (e) =>
            //{

            //}, WorkItemPriority.Normal);
        }


        /// <summary>
        /// 显示范围改变
        /// </summary>
        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            _currentRange = visibleRange;
            if (_allLoaded) return;
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
                var results = await _fileQueryResult.GetFilesAsync((uint)range.FirstIndex, range.Length).AsTask(ct);
                if (results != null)
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        ct.ThrowIfCancellationRequested();
                        var t = Items.First(x => x.LocalPath == results[i].Path);
                        if (!t.Loaded)
                        {
                            var thumb = await t.SetPreviewImage(ct);
                            var index = Items.IndexOf(t);
                            if (thumb != null && index <= _currentRange.LastIndex && index >= _currentRange.FirstIndex)
                            {
                                t.CachePath = thumb;
                            }
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
                     where x.IndexRange.FirstIndex - 5 > range.LastIndex || x.IndexRange.LastIndex + 5 < range.FirstIndex // 范围不同
                     select x).ToList();
            foreach (var request in r)
            {
                _requesting.Remove(request); // 移出请求
                request.Cancel();
            }

            Debug.WriteLine("还剩 " + _requesting.Count + " 个请求");
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
