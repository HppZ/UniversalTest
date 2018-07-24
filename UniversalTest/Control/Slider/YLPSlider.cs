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
        private SliderState _currentState = SliderState.None;

        public event EventHandler<StateChangedArgs> StateChangedEvent;

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            "State", typeof(SliderState), typeof(YLPSlider), new PropertyMetadata(default(SliderState)));
        public SliderState State
        {
            get { return (SliderState)GetValue(StateProperty); }
            set
            {
                throw new InvalidOperationException();
                SetValue(StateProperty, value);
            }
        }

        public YLPSlider()
        {
            this.ManipulationMode = ManipulationModes.TranslateX;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_hThumb == null)
            {
                _hThumb = GetTemplateChild(HorizontalThumbPart) as Thumb;
                _hThumb.ManipulationMode = ManipulationModes.None;

                _hThumb.DragStarted += HThumbDragStarted;
                _hThumb.DragDelta += HThumbDragDelta;
                _hThumb.DragCompleted += HThumbDragCompleted;
            }
        }

        #region manipulation
        protected override void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
            //base.OnManipulationStarting(e);
            Debug.WriteLine("OnManipulationStarting");
        }

        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            //base.OnManipulationStarted(e);
            Debug.WriteLine("OnManipulationStarted");
            RaiseStateChangedEvent(SliderState.Pressed);
        }

        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e)
        {
            //base.OnManipulationInertiaStarting(e);
            Debug.WriteLine("OnManipulationInertiaStarting");
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            //base.OnManipulationDelta(e);
            Debug.WriteLine("OnManipulationDelta");
            RaiseStateChangedEvent(SliderState.Moving);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            //base.OnManipulationCompleted(e);
            Debug.WriteLine("OnManipulationCompleted");
            RaiseStateChangedEvent(SliderState.Released);
        }
        #endregion

        #region thumb
        private void HThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            Debug.WriteLine("HThumbDragStarted");
            RaiseStateChangedEvent(SliderState.Pressed);
        }

        private void HThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            Debug.WriteLine("HThumbDragDelta");
            RaiseStateChangedEvent(SliderState.Moving);
        }

        private void HThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            Debug.WriteLine("HThumbDragCompleted");
            RaiseStateChangedEvent(SliderState.Released);
        }
        #endregion

        private void RaiseStateChangedEvent(SliderState state)
        {
            if (_currentState != state)
            {
                _currentState = state;
                StateChangedEvent?.Invoke(this, new StateChangedArgs(_currentState));
            }
        }
    }


    public enum SliderState
    {
        None,
        Pressed,
        Moving,
        Released,
    }

    public class StateChangedArgs : EventArgs
    {
        public StateChangedArgs(SliderState state)
        {
            State = state;
        }

        public SliderState State { get; }
    }

}


//DragEnter;
//DragOver;
//DragLeave;
//DragStarting;
