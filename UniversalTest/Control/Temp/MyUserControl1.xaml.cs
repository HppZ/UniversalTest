using System;
using System.Collections.Generic;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Win10
{
    public sealed partial class MyUserControl1 : UserControl
    {
        public MyUserControl1()
        {
            this.InitializeComponent();
        }
        public event Action<object, double> ValueChanged; // 滚动条滚动值变化


        //public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        //    "Value", typeof (double), typeof (MyUserControl1), new PropertyMetadata(default(double)));

        //public double Value
        //{
        //    get { return (double) GetValue(ValueProperty); }
        //    set { SetValue(ValueProperty, value); }
        //}

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof (Double), typeof (MyUserControl1), new PropertyMetadata(default(Double)));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bi = (d as MyUserControl1).GetBindingExpression(MyUserControl1.ValueProperty);


            //(d as MyUserControl1).ValueChanged?.Invoke(null,(double)e.NewValue);
        }


        public Double Value
        {
            get { return (Double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        private void RangeBase_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var bi = this.GetBindingExpression(MyUserControl1.ValueProperty);
            ValueChanged?.Invoke(null,e.NewValue);
        }
    }
}
