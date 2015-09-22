using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Slider
{
    public sealed partial class BallSlider : UserControl
    {
        public BallSlider()
        {
            this.InitializeComponent();
            this.SizeChanged += BallSlider_SizeChanged;
        }

        private void BallSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var mode = UIViewSettings.GetForCurrentView().UserInteractionMode;
            if (mode == UserInteractionMode.Mouse)
            {
                Debug.WriteLine("mouse");
            }
            else if (mode == UserInteractionMode.Touch)
            {
                Debug.WriteLine("touch");
            }

        }

    }
}
