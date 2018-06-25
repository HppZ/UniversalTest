using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage31 : Page
    {
        public BlankPage31()
        {
            this.InitializeComponent();
        }

        private void BlankPage31_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private async void BlankPage31_Drop(object sender, DragEventArgs e)
        {
            _folder = null;
            _files = null;

            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count <= 0) return;

                if (items.Count == 1)
                {
                    var folder = items[0] as StorageFolder;
                    OnGetFolder(folder);
                }
                else
                {
                    var files = items.Select(x => x as StorageFile);
                    OnGetFiles(files);
                }
            }
        }

        private StorageFolder _folder = null;
        private async void OnGetFolder(StorageFolder folder)
        {
            _folder = folder;
            TipElement.Text = _folder.DisplayName;

            var files = await folder.GetFilesAsync();
            OnGetFiles(files);
        }

        private List<FileItem> _files = null;
        private async void OnGetFiles(IEnumerable<StorageFile> files)
        {
            TipElement.Text = files.First().DisplayName;

            _files = new List<FileItem>();
            foreach (var storageFile in files)
            {
                var width = (await storageFile.Properties.GetImagePropertiesAsync()).Width;
                _files.Add(new FileItem(storageFile, width));
            }
        }


        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SetFileNameTextBox.Text))
                return;

            var filename = SetFileNameTextBox.Text;

            Task.Run(async () =>
            {
                if (_files != null && _files.Any())
                {
                    var files = _files.OrderBy(x => x.Width);
                    var smalllest = files.First();
                    var smalllestWidth = smalllest.Width;

                    var parentFolder = await smalllest.StorageFile.GetParentAsync();
                    foreach (var file in _files)
                    {
                        var width = file.Width;
                        var scale = width / smalllestWidth * 100;
                        var scaleFolder = await parentFolder.CreateFolderAsync($"Scale-{scale}", CreationCollisionOption.OpenIfExists);
                        var newFIle = await file.StorageFile.CopyAsync(scaleFolder);
                        await newFIle.RenameAsync(filename + newFIle.FileType, NameCollisionOption.ReplaceExisting);
                    }
                }
            });

        }
    }


    class FileItem
    {
        public StorageFile StorageFile { get; set; }
        public double Width { get; set; }

        public FileItem(StorageFile file, double w)
        {
            StorageFile = file;
            Width = w;
        }
    }
}
