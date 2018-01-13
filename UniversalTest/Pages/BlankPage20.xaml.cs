using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage20 : Page
    {
        List<BitmapImage> list = new List<BitmapImage>();
        public BlankPage20()
        {
            this.InitializeComponent();
            Tapped += BlankPage20_Tapped;
            Loaded += BlankPage20_Loaded;


        }

        private async void BlankPage20_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 120; i++)
            {
                var uri = new Uri("ms-appx:///Assets/inner/inner_" + i + ".png");
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                using (var s = await file.OpenReadAsync())
                {
                    var b = new BitmapImage();
                    await b.SetSourceAsync(s);
                    list.Add(b);
                }
            }
        }

        private Storyboard sb;

        private void BlankPage20_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sb == null)
            {
                sb = new Storyboard()
                {
                };
                Storyboard.SetTarget(sb, ImageElement);
                Storyboard.SetTargetProperty(sb, "Source");
                var of = new ObjectAnimationUsingKeyFrames()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(3))
                };
                sb.Children.Add(of);

                for (int i = 0; i < 120; i++)
                {
                    var df = new DiscreteObjectKeyFrame()
                    {
                        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3 / 120.0 * i)),
                        Value = list.ElementAt(i)
                    };

                    of.KeyFrames.Add(df);
                }
            }
            sb.Begin();
        }



    }
}
