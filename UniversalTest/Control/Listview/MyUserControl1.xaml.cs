using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Media3D;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.ListView
{
    public sealed partial class MyUserControl1 : UserControl
    {
        public MyUserControl1()
        {
            this.InitializeComponent();
            Loaded += MyUserControl1_Loaded;
            manuGrid.ManipulationMode = ManipulationModes.All;
            manuGrid.ManipulationDelta += MyUserControl1_ManipulationDelta;
            CompositeTransform3D1.RegisterPropertyChangedCallback(CompositeTransform3D.RotationYProperty, OnChanged);
        }

        private void MyUserControl1_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            
            // if (e.IsInertial)
            {
                //      Debug.WriteLine("惯性了");
                var a = CompositeTransform3D1.RotationY + e.Delta.Translation.X;
                CompositeTransform3D1.RotationY = a % 360;
            }
            Debug.WriteLine(CompositeTransform3D1.RotationY);
            //(Storyboard1.Children[0] as DoubleAnimation).To =;
            //Storyboard1.Begin();
        }

        private void OnChanged(DependencyObject sender, DependencyProperty dp)
        {
           // Storyboard1.Pause();
            var rotation = CompositeTransform3D1.RotationY;
            var t = ItemsControl1.ItemsPanelRoot as Grid;
            Debug.WriteLine("rotation " + rotation);


            foreach (var c in t.Children)
            {
                var c1 = (c as ContentControl).ContentTemplateRoot as Grid;
                var angle = (c1.RenderTransform as RotateTransform).Angle;

                if (angle == 0)
                {
                    var l = c1.Children[0] as Line;
                    l.Stroke = new SolidColorBrush(Colors.Red);
                }

                if (rotation > 90 && rotation < 270)
                {
                    Debug.WriteLine("B");

                    var x = rotation - 90;
                    var a = x;
                    var b = 180 + x;

                    if (angle >= a && angle <= b)
                    {
                        c1.Opacity = 1;
                    }
                    else
                    {
                        c1.Opacity = 0;
                    }
                }
                else
                {
                    Debug.WriteLine("A");
                    var x = rotation;

                    if (x >= 270)
                    {
                        x = x - 270;
                        if ((angle >= 0 && angle <= x) || (angle >= 180 + x && angle <= 360))
                        {
                            c1.Opacity = 1;
                        }
                        else
                        {
                            c1.Opacity = 0;
                        }
                    }
                    else
                    {
                        if ((angle >= 0 && angle <= 90 + x) || (angle >= 270 + x && angle <= 360))
                        {
                            c1.Opacity = 1;
                        }
                        else
                        {
                            c1.Opacity = 0;
                        }
                    }

                }
            }

         //   Storyboard1.Resume();
        }


        private void MyUserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            //  CompositeTransform3D1.ScaleX = CompositeTransform3D1.ScaleY = -CompositeTransform3D1.TranslateZ / PerspectiveTransform.Depth + 1.0;

            CompositeTransform3D1.CenterX = 300;
            CompositeTransform3D1.CenterY = 300;
           // Storyboard1.Begin();
        }
    }

    class ItemsControl2 : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ContentControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ContentControl;
        }
    }

    public class AngleSource
    {
        public IEnumerable<double> Items
        {
            get
            {
                return Enumerable.Range(0, 60).Select(d => d * 6.0);
            }
        }
    }

    class RotationToOpacityConverter : IValueConverter
    {
        private int i = 0;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var angle = (parameter as RotateTransform).Angle;
            var rotation = (double)value;
            var a = -90 - rotation;
            var b = 90 - rotation;
            Debug.WriteLine("angle " + angle);
            Debug.WriteLine("rotation " + rotation);
            if (angle >= a && angle <= b) // 前
            {
                return 1.0;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }


}
