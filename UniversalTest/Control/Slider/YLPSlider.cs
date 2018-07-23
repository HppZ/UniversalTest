using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [TemplatePart(Name = HorizontalThumbPart, Type = typeof(Thumb))]
    [TemplatePart(Name = VerticalThumbPart, Type = typeof(Thumb))]
    public sealed class YLPSlider : Windows.UI.Xaml.Controls.Slider
    {
        private const string HorizontalThumbPart = "HorizontalThumb";
        private const string VerticalThumbPart = "VerticalThumb";

        private Thumb _hThumb;

        public event EventHandler<StateChangedArgs> StateChangedEvent;

        public static readonly DependencyProperty ThumbStateProperty = DependencyProperty.Register(
            "ThumbState", typeof(SliderThumbState), typeof(YLPSlider), new PropertyMetadata(default(SliderThumbState)));
        public SliderThumbState ThumbState
        {
            get { return (SliderThumbState)GetValue(ThumbStateProperty); }
            set
            {
                throw new InvalidOperationException();
                SetValue(ThumbStateProperty, value);
            }
        }

        public YLPSlider()
        {
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_hThumb == null)
            {
                _hThumb = GetTemplateChild(HorizontalThumbPart) as Thumb;

                _hThumb.DragStarted += HThumbDragStarted;
                _hThumb.DragDelta += HThumbDragDelta;
                _hThumb.DragCompleted += HThumbDragCompleted;
            }
        }

        private void HThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            Debug.WriteLine("HThumbDragStarted");
            RaiseStateChangedEvent(SliderThumbState.ThumbPressed);
        }

        private void HThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            Debug.WriteLine("HThumbDragDelta");
            RaiseStateChangedEvent(SliderThumbState.ThumbMoving);
        }

        private void HThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            Debug.WriteLine("HThumbDragCompleted");
            RaiseStateChangedEvent(SliderThumbState.ThumbReleased);
        }

        private void RaiseStateChangedEvent(SliderThumbState reason)
        {
            StateChangedEvent?.Invoke(this, new StateChangedArgs(reason));
        }
    }


    public enum SliderThumbState
    {
        ThumbPressed,
        ThumbMoving,
        ThumbReleased,
    }

    public class StateChangedArgs : EventArgs
    {
        public StateChangedArgs(SliderThumbState state)
        {
            State = state;
        }

        public SliderThumbState State { get; }
    }

}


//DragEnter;
//DragOver;
//DragLeave;
//DragStarting;
