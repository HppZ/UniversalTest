using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Sight.Windows10.Helper
{
    public static class VisualTreeExtensions
    {
        #region GetVisualChildren(...)
        
        internal static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (var counter = 0; counter < childCount; counter++)
            {
                yield return VisualTreeHelper.GetChild(parent, counter);
            }
        }
        #endregion

        #region GetLogicalChildrenBreadthFirst(...)
        internal static IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(this FrameworkElement parent)
        {
            var queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();

                yield return element;

                foreach (var visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
                {
                    queue.Enqueue(visualChild);
                }
            }
        }
        #endregion


        #region GetLogicalParent
        internal static T GetLogicalParent<T>(this FrameworkElement childElement) where T : FrameworkElement
        {
            if (childElement == null) return default(T);
            FrameworkElement child = childElement;
            while (child != null)
            {
                //System.Diagnostics.Debug.WriteLine("[GetLogicalParent] " + child.ToString());
                child = child.Parent as FrameworkElement;
                if (child is T) return child as T;
            }
            return default(T);
        }
        #endregion

        public static T FindFirstElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
        {
            if (parentElement != null)
            {
                var count = VisualTreeHelper.GetChildrenCount(parentElement);
                if (count == 0)
                    return null;

                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parentElement, i);

                    if (child != null && child is T)
                        return (T)child;
                    else
                    {
                        var result = FindFirstElementInVisualTree<T>(child);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }

}
