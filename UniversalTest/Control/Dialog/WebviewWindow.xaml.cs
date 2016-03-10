using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sight.Windows10.Views.Album
{
    public sealed partial class WebviewWindow : UserControl
    {
        #region fields
        private Popup _popup;
        #endregion

        #region ctor
        public WebviewWindow()
        {
            this.InitializeComponent();

            _popup = new Popup()
            {
                Child = this,
                IsLightDismissEnabled = false,
            };

            this.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.ManipulationDelta += WebviewWindow_ManipulationDelta;

            Loaded += WebviewWindow_Loaded;
            Unloaded += WebviewWindow_Unloaded;
        }
        #endregion


        #region property
        /// <summary>
        /// 窗口标题
        /// </summary>
        public static readonly DependencyProperty TitleStringProperty = DependencyProperty.Register(
            "TitleString", typeof(string), typeof(WebviewWindow), new PropertyMetadata(default(string)));

        public string TitleString
        {
            get { return (string)GetValue(TitleStringProperty); }
            set { SetValue(TitleStringProperty, value); }
        }
        #endregion

        #region public
        /// <summary>
        /// 设置跳转地址
        /// </summary>
        public void SetUri(Uri uri)
        {
            WebViewElement.Navigate(uri);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            AdaptWindowSize();
            _popup.IsOpen = true;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            _popup.IsOpen = false;
        }
        #endregion

        #region private
        /// <summary>
        /// 适应位置/大小
        /// </summary>
        private void AdaptWindowSize()
        {
            var size = Window.Current.Bounds;
            // 宽度固定440
            this.Width = 440;
            this.Height = size.Height * 0.8;

            // 定位popup
            _popup.HorizontalOffset = size.Width - this.Width - 100;
            _popup.VerticalOffset = size.Height / 2 - this.Height / 2;
        }
        #endregion

        #region UI events
        /// <summary>
        /// 关闭
        /// </summary>
        private void Close_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Close();
        }

        private void WebviewWindow_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_popup.IsOpen)
            {
                _popup.HorizontalOffset += e.Delta.Translation.X;
                _popup.VerticalOffset += e.Delta.Translation.Y;
            }
        }
        #endregion

        #region shortcut(ESC) + sizechange
        private void WebviewWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            this.Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
        }

        private void WebviewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            this.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private void Dispatcher_AcceleratorKeyActivated(Windows.UI.Core.CoreDispatcher sender, Windows.UI.Core.AcceleratorKeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape)
            {
                Close();
            }
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            AdaptWindowSize();
        }
        #endregion

        #region shadow
        private void RootGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShadowGrid.Width = e.NewSize.Width;
            ShadowGrid.Height = e.NewSize.Height;
        }
        #endregion

    }
}
