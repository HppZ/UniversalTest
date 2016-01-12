using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UniversalTest.Pages;

namespace UniversalTest.Control.Panel
{

    public class CustomPanel : Windows.UI.Xaml.Controls.Panel
    {
        #region ItemHeight
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                "ItemHeight",
                typeof(double),
                typeof(CustomPanel),
                new PropertyMetadata(double.NaN, OnItemHeightOrWidthPropertyChanged));
        #endregion public double ItemHeight

        private static void OnItemHeightOrWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomPanel panel = (CustomPanel)d;
            panel.InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size lineSize = new Size();
            Size totalSize = new Size();
            Size maximumSize = new Size(availableSize.Width, availableSize.Height);

            double itemHeight = ItemHeight;
            foreach (UIElement element in Children)
            {
                var itemContent = (element as GridViewItem).Content; // TODO don't reference GridViewItem
                var itemImage = itemContent as ISize;
                double ratio = 1.0 * itemImage.Width / itemImage.Height; // 图片比例
                Size itemSize = new Size(itemHeight * ratio, itemHeight);

                element.Measure(itemSize);
                Size elementSize = new Size(element.DesiredSize.Width, element.DesiredSize.Height);

                if (NumbericCompareExtensions.IsGreaterThan(lineSize.Width + elementSize.Width, maximumSize.Width))
                {
                    totalSize.Height += lineSize.Height;

                    lineSize = elementSize;

                    if (NumbericCompareExtensions.IsGreaterThan(elementSize.Width, maximumSize.Width))
                    {
                        totalSize.Height += elementSize.Height;
                        lineSize = new Size();
                    }
                }
                else
                {
                    lineSize.Width += elementSize.Width;
                    lineSize.Height = elementSize.Height;
                }
            }

            totalSize.Height += lineSize.Height;
            return new Size(availableSize.Width, totalSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size lineSize = new Size();
            Size maximumSize = new Size(finalSize.Width, finalSize.Height);

            double indirectOffset = 0;

            UIElementCollection children = Children;
            int count = children.Count;
            int lineStart = 0;
            for (int lineEnd = 0; lineEnd < count; lineEnd++)
            {
                UIElement element = children[lineEnd];
                Size elementSize = new Size(element.DesiredSize.Width, element.DesiredSize.Height);

                if (NumbericCompareExtensions.IsGreaterThan(lineSize.Width + elementSize.Width, maximumSize.Width))
                {
                    ArrangeLine(lineStart, lineEnd, indirectOffset, lineSize.Height, maximumSize.Width / lineSize.Width);

                    indirectOffset += lineSize.Height;
                    lineSize = elementSize;

                    if (NumbericCompareExtensions.IsGreaterThan(elementSize.Width, maximumSize.Width))
                    {
                        ArrangeLine(lineEnd, ++lineEnd, indirectOffset, elementSize.Height, maximumSize.Width / elementSize.Width);

                        indirectOffset += lineSize.Height;
                        lineSize = new Size();
                    }

                    lineStart = lineEnd;
                }
                else
                {
                    lineSize.Width += elementSize.Width;
                    lineSize.Height = Math.Max(lineSize.Height, elementSize.Height);
                }
            }

            if (lineStart < count)
            {
                ArrangeLine(lineStart, count, indirectOffset, lineSize.Height, 1);
            }

            return finalSize;
        }

        private void ArrangeLine(int lineStart, int lineEnd, double indirectOffset, double indirectGrowth, double scale)
        {
            double directOffset = 0.0;
            UIElementCollection children = Children;
            for (int index = lineStart; index < lineEnd; index++)
            {
                UIElement element = children[index];
                Size elementSize = new Size(element.DesiredSize.Width, element.DesiredSize.Height);
                double directGrowth = elementSize.Width;

                Rect bounds = new Rect(directOffset, indirectOffset, directGrowth * scale, indirectGrowth);
                element.Arrange(bounds);

                directOffset += directGrowth * scale;
            }
        }
    }


    internal static class NumbericCompareExtensions
    {
        public static bool IsGreaterThan(double left, double right)
        {
            return (left > right) && !AreClose(left, right);
        }

        public static bool AreClose(double left, double right)
        {
            if (left == right)
            {
                return true;
            }

            double a = (Math.Abs(left) + Math.Abs(right) + 10.0) * 2.2204460492503131E-16;
            double b = left - right;
            return (-a < b) && (a > b);
        }
    }
}