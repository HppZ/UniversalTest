using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class BlankPage12 : Page
    {
        public BlankPage12()
        {
            this.InitializeComponent();
            Loaded += BlankPage12_Loaded;
        }

        private async void BlankPage12_Loaded(object sender, RoutedEventArgs e)
        {
            var file = await KnownFolders.PicturesLibrary.GetFileAsync("333.jpg");
            EditFile(file);
        }


        private async void EditFile(StorageFile file)
        {
            IRandomAccessStream ras = null;
            string name = "照片";
            try
            {
                ras = await file.OpenAsync(FileAccessMode.ReadWrite);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(ras);

                try
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateForInPlacePropertyEncodingAsync(decoder);
                    var propertySet = new BitmapPropertySet
                    {
                        {"System.ApplicationName", new BitmapTypedValue(name, PropertyType.String)}
                    };
                    await encoder.BitmapProperties.SetPropertiesAsync(propertySet);
                    await encoder.FlushAsync();
                }
                catch (Exception e)
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);
                    var propertySet = new BitmapPropertySet
                    {
                        {"System.ApplicationName", new BitmapTypedValue(name, PropertyType.String)}
                    };
                    await encoder.BitmapProperties.SetPropertiesAsync(propertySet);
                    await encoder.FlushAsync();
                }

            }
            catch (Exception e)
            {
            }
            finally
            {
                ras?.Dispose();
            }
        }
    }
}
