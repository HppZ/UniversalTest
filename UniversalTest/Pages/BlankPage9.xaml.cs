using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage9 : Page
    {
        private ObservableCollection<ImageItem1> source;
        public BlankPage9()
        {
            this.InitializeComponent();
            this.Loaded += BlankPage9_Loaded;
        }

        private async void BlankPage9_Loaded(object sender, RoutedEventArgs e)
        {
            source = new ObservableCollection<ImageItem1>();
            this.DataContext = source;

            QueryOptions options = new QueryOptions(CommonFileQuery.OrderBySearchRank, new List<string>() { ".jpg", ".png" });
            var query = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(options);
            var files = await query.GetFilesAsync();
            foreach (var file in files)
            {
                var thumb = await file.GetThumbnailAsync(ThumbnailMode.SingleItem);
                var bmp = new BitmapImage();
                bmp.SetSource(thumb);
                source.Add(new ImageItem1()
                {
                    PreviewImage = bmp,
                });
            }
        }

    }


    interface ISize
    {
        double Width { get; }
        double Height { get; }
    }

    class ImageItem1 : ISize
    {
        public BitmapImage PreviewImage { set; get; }
        public double Width
        {
            get { return PreviewImage.PixelWidth; }

        }

        public double Height
        {
            get { return PreviewImage.PixelHeight; }
        }
    }

}
