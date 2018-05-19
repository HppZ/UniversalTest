using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using Windows.Data.Json;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage27 : Page
    {
        private StorageFolder _iconFolder;

        public BlankPage27()
        {
            this.InitializeComponent();
            Loaded += BlankPage27_Loaded;
        }

        private async void BlankPage27_Loaded(object sender, RoutedEventArgs e)
        {
            _iconFolder = await KnownFolders.PicturesLibrary.CreateFolderAsync("svg", CreationCollisionOption.OpenIfExists);
        }

        private async void GetIcon_Click(object sender, RoutedEventArgs e)
        {
            ErrorElement.Text = string.Empty;

            var id = IconIdElement.Text;
            try
            {
                var http = new HttpClient();
                var result = await http.GetStringAsync($"https://api.icons8.com/api/iconsets/icon?id={id}");

                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(result);

                var icon = xmldoc.GetElementsByTagName("icon")[0];
                var name = icon.Attributes["name"].Value;
                var iconid = icon.Attributes["id"].Value;

                var svgBase64 = icon.ChildNodes[0].InnerText;
                string svg = Encoding.UTF8.GetString(Convert.FromBase64String(svgBase64));

                var file = await _iconFolder.CreateFileAsync(name + "_" + iconid + ".svg", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, svg);
            }
            catch (Exception exception)
            {
                ErrorElement.Text = $"{id}" + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace;
            }
        }







    }
}
