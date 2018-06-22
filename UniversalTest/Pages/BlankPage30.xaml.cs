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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage30 : Page
    {

        private Storyboard _scrollStoryboard;
        private DoubleAnimation _scrollAnimation;
        private TranslateTransform _translate;

        public BlankPage30()
        {
            this.InitializeComponent();
            _translate = new TranslateTransform();
            TestElement.RenderTransform = _translate;
            EnsureAnimationObject();
        }

        private void EnsureAnimationObject()
        {
            if (_scrollStoryboard == null)
            {
                _scrollStoryboard = new Storyboard();
                _scrollStoryboard.Completed += _scrollStoryboard_Completed;
                _scrollAnimation = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(10000)),
                };
                Storyboard.SetTarget(_scrollStoryboard, _translate);
                Storyboard.SetTargetProperty(_scrollStoryboard, "(TranslateTransform.X)");
                _scrollStoryboard.Children.Add(_scrollAnimation);
            }
        }

        private void _scrollStoryboard_Completed(object sender, object e)
        {
            Debug.WriteLine("completed");
        }

        private void seekOnClick(object sender, RoutedEventArgs e)
        {
            _scrollStoryboard.Seek(TimeSpan.FromSeconds(1.5));
        }

        private void stopOnClick(object sender, RoutedEventArgs e)
        {
            _scrollStoryboard.Stop();
        }

        private void seekAlignedToLastTickOnClick(object sender, RoutedEventArgs e)
        {
            _scrollStoryboard.SeekAlignedToLastTick(TimeSpan.FromSeconds(1.5));
        }

        private void seekAlignedOnClick(object sender, RoutedEventArgs e)
        {
            _scrollStoryboard.SeekAlignedToLastTick(TimeSpan.FromSeconds(10000));
            Debug.WriteLine("SeekAlignedToLastTick");
        }

        private void skiptofillOnClick(object sender, RoutedEventArgs e)
        {
            _scrollStoryboard.SkipToFill();
            Debug.WriteLine("skip to fill");
        }

        private void startOnClick(object sender, RoutedEventArgs e)
        {
            _scrollAnimation.To = 1000;
            _scrollStoryboard.Begin();
        }

        private void resetOnClick(object sender, RoutedEventArgs e)
        {
            _translate.X = 0;
        }

        
    }
}
