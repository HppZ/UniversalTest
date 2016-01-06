using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UniversalTest.Model
{
    /// <summary>
    /// impl IItemsRangeInfo
    /// </summary>
    public class RangeCollection<T> : ObservableCollection<T>, IItemsRangeInfo
    {
        
        public RangeCollection(IEnumerable<T> collection):base(collection)
        {
        }

        public void Dispose()
        {

        }

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            Debug.WriteLine("first "+visibleRange.FirstIndex +"  last " + visibleRange.LastIndex);
        }
    }
}
