using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class BlankPage4 : Page
    {
        public BlankPage4()
        {
            this.InitializeComponent();
        }


        //--------------------------------------------------------------------------
        /// <summary>
        /// 这里发现: 对slider快速上下滑动, 放开, 再迅速快速滑动,再放开,重复操作会发生在拖动滚动条时_thumb?.IsDragging为false
        /// </summary>
        private int i = 0;
        private void RangeBase_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Debug.WriteLine("value " + i++ + " drag " + _thumb?.IsDragging);
        }

        private Thumb _thumb;
        private void VerticalThumb_OnLoaded(object sender, RoutedEventArgs e)
        {
            _thumb = sender as Thumb;
        }
        //--------------------------------------------------------------------------

    }
}
