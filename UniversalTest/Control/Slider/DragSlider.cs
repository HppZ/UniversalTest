using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UniversalTest.Control.Slider
{
    [TemplatePart(Name = ThumbPart, Type = typeof(FrameworkElement))]
    public sealed class DragSlider : Windows.UI.Xaml.Controls.Control
    {

        public event EventHandler<StateChangedArgs> StateChanged;

        private const string ThumbPart = "Thumb";

        private FrameworkElement _thumb;
        public DragSlider()
        {
            this.DefaultStyleKey = typeof(DragSlider);
        }


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _thumb = GetTemplateChild(ThumbPart) as FrameworkElement;
            _thumb.ManipulationMode = ManipulationModes.TranslateX;

            _thumb.ManipulationStarting += _thumb_ManipulationStarting;
            _thumb.ManipulationDelta += _thumb_ManipulationDelta;
            _thumb.ManipulationCompleted += _thumb_ManipulationCompleted;

        }


        private void _thumb_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {

        }

        private void _thumb_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

        }

        private void _thumb_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            
        }
    }


    public enum StateChangedReason
    {
        ThumbPressed,
        ThumbMoving,
        ThumbReleased,
    }

    public class StateChangedArgs : EventArgs
    {
        public StateChangedArgs(StateChangedReason reason)
        {
            Reason = reason;
        }

        public StateChangedReason Reason { get; }
    }

}
