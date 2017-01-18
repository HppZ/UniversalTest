using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage11 : Page
    {
        public BlankPage11()
        {
            this.InitializeComponent();
            Loaded += BlankPage11_Loaded;
        }

        private async void BlankPage11_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await DoWork();
            }
            catch (Exception)
            {
            }
        }

        private async Task DoWork()
        {
            var f = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/giftest.gif"));
            f = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/vedio.mp4"));
            f = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/live.jpg"));

            MemoryStream ms = null;
            using (var s = await f.OpenReadAsync())
            {
                using (var zip = new ZipArchive(s.AsStream(), ZipArchiveMode.Read))
                {
                    ZipArchiveEntry entry = zip.GetEntry("formats/living/living.mp4");
                    if (entry != null)
                    {
                        using (var st = entry.Open())
                        {
                            ms = new MemoryStream();
                            st.CopyTo(ms);
                            ms.Flush();
                        }
                    }
                }
            }
            var m = ms.AsRandomAccessStream();
            m.Seek(0);

            MediaElement1.SetSource(m, "video/mp4");
            MediaElement1.Play();
        }

    }
}
