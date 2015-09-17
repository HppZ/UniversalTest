using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
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
                    Path = file.Path
                });
            }
        }
    }


    public class ImageItem : INotifyPropertyChanged
    {
        public string Path { get; set; }

        public BitmapImage PreviewImage
        {
            get
            {
                BitmapImage bitmap = new BitmapImage();
                SetPreviewImage(bitmap);
                return bitmap;
            }
            set
            {
                OnPropertyChanged();
            }
        }

        private async void SetPreviewImage(BitmapImage bitmap)
        {
            var file = await StorageFile.GetFileFromPathAsync(Path);
            var thumb = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 500);
            bitmap.SetSource(thumb);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
