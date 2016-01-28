using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace UniversalTest.Control.Progress
{
    // 方法1 这里就是
    // 放法2 用渐变 gradient brush
    public sealed class ProgressButton : Windows.UI.Xaml.Controls.Button
    {
        private Border _progressBorder = null;
        private Border _border = null;
        public ProgressButton()
        {
            this.DefaultStyleKey = typeof(ProgressButton);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _progressBorder = GetTemplateChild("ProgressBorder") as Border;
            _border = GetTemplateChild("Border") as Border;

            this.SizeChanged += ProgressButton_SizeChanged;
        }

        void ProgressButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeWidth();
        }

        /// <summary>
        /// set or get value of progressbar, max is 100
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(ProgressButton), new PropertyMetadata(default(double), ValueChangedCallback));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ele = d as ProgressButton;
            ele.ChangeWidth();
        }

        /// <summary>
        /// CornerRadius of this button
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(ProgressButton), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private void ChangeWidth()
        {
            if (this.Value > 100)
            {
                this.Value = 100;
            }
            if (this.Value < 0)
            {
                this.Value = 0;
            }

            if (this._progressBorder != null)
            {
                this._progressBorder.Width = (this.Value / 100.0) * this.ActualWidth;
            }
            if (this._border != null)
            {
                this._border.Width = ActualWidth;
            }
        }
    }
}
