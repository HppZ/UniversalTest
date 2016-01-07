using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.BulkAccess;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using UniversalTest.Annotations;
using UniversalTest.Model;

namespace UniversalTest.Controller
{
    public class MainController
    {
        public RangeCollection Source;

        //public ObservableCollection<ImageItem> Source = new ObservableCollection<ImageItem>();

        public async Task Init()
        {
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByDate,
                new string[] { ".jpg", ".png", ".jpeg", ".bmp" })
            {
                FolderDepth = FolderDepth.Deep,
                IndexerOption = IndexerOption.OnlyUseIndexer,
                UserSearchFilter = "System.Kind:=System.Kind#Picture"
            };
            queryOptions.SetThumbnailPrefetch(ThumbnailMode.SingleItem, 256, ThumbnailOptions.UseCurrentScale);
            var _fileQueryResult = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(queryOptions);
            var files = await _fileQueryResult.GetFilesAsync();
            Debug.WriteLine("Count "+files.Count);
            var list = new List<ImageItem>();
            foreach (var f in files)
            {
                list.Add(new ImageItem()
                {
                    LocalPath =  f.Path
                });
            }

            Source = new RangeCollection(list);
        }

        //private async Task GetFilesInPictureLibrary()
        //{
        //    QueryOptions options = new QueryOptions(CommonFileQuery.OrderBySearchRank, new List<string>() { ".jpg", ".png" });
        //    var query = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(options);
        //    var files = await query.GetFilesAsync();
        //    foreach (var file in files)
        //    {
        //        Source.Add(new ImageItem()
        //        {
        //            LocalPath = file.Path,
        //        });
        //    }
        //}
    }


    public class ImageItem : INotifyPropertyChanged
    {
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public string LocalPath { get; set; }

        public BitmapImage PreviewImage
        {
            get
            {
                if (cachePath != null)
                {
                    return new BitmapImage(cachePath);
                }
                return null;
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public bool Loaded { get; set; }

        public Uri cachePath;
        public Uri CachePath
        {
            get
            {
                return cachePath;
            }
            set
            {
                cachePath = value;
                //OnPropertyChanged();
                OnPropertyChanged("PreviewImage");
            }
        }

        public async Task SetPreviewImage(CancellationToken ct)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (Loaded) return;
                var file = await StorageFile.GetFileFromPathAsync(LocalPath).AsTask(ct);
                var thumb = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 256).AsTask(ct); // 如果已经拿到thumbnail则不在cancel了，直接保存
                //var name = Path.GetFileName(LocalPath);
                var name = Path.GetRandomFileName() + ".jpg";
                var cacheFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting).AsTask(ct);

                Windows.Storage.Streams.Buffer buffer = new Windows.Storage.Streams.Buffer(Convert.ToUInt32(thumb.Size));
                IBuffer iBuf = await thumb.ReadAsync(buffer, buffer.Capacity, InputStreamOptions.None).AsTask(ct);
                using (var strm = await cacheFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await strm.WriteAsync(iBuf);
                    await strm.FlushAsync();
                }
                CachePath = new Uri("ms-appdata:///Local" + "/" + cacheFile.Name);
                Loaded = true;
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
