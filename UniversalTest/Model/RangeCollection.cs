using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UniversalTest.Model
{
    /// <summary>
    /// impl IItemsRangeInfo
    /// </summary>
    public class RangeCollection: IItemsRangeInfo
    {
        public void Dispose()
        {
        }

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
        }
    }
}
