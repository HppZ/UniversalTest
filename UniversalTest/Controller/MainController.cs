using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Windows.UI.Xaml.Media.Imaging;
using UniversalTest.Annotations;

namespace UniversalTest.Controller
{
    public class MainController
    {
        public ObservableCollection<ImageItem> Source = new ObservableCollection<ImageItem>();

        public async Task Init()
        {
            await GetFilesInPictureLibrary();
        }

        private async Task GetFilesInPictureLibrary()
        {
            QueryOptions options = new QueryOptions(CommonFileQuery.OrderBySearchRank, new List<string>() { ".jpg", ".png" });
            var query = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(options);
            var files = await query.GetFilesAsync();
            foreach (var file in files)
            {
                Source.Add(new ImageItem()
                {
                    LocalPath = file.Path,
                });
            }
        }
    }


    public class ImageItem : INotifyPropertyChanged
    {
        public ImageItem()
        {
            PreviewImage = new BitmapImage();
        }

        public string LocalPath { get; set; }

        public BitmapImage PreviewImage { get; set; }

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
                OnPropertyChanged();
            }
        }

        private async Task SetPreviewImage()
        {
            var file = await StorageFile.GetFileFromPathAsync(LocalPath);
            var thumb = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 500);
            var name = Path.GetFileName(LocalPath);
            var cacheFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            CachePath = new Uri(ApplicationData.Current.LocalFolder.Path + "\\" + cacheFile.Name);

            Windows.Storage.Streams.Buffer buffer = new Windows.Storage.Streams.Buffer(Convert.ToUInt32(thumb.Size));
            IBuffer iBuf = await thumb.ReadAsync(buffer, buffer.Capacity, InputStreamOptions.None);
            using (var strm = await cacheFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await strm.WriteAsync(iBuf);
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
