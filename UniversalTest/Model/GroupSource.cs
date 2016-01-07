using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UniversalTest.Model
{
    public class GroupSource:ObservableCollection<GroupSourceItem>, IItemsRangeInfo
    {
        public void Dispose()
        {
        }

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            throw new NotImplementedException();
        }
    }


    public class GroupSourceItem:IItemsRangeInfo
    {
        public string Key { get; set; }  
        public ObservableCollection<int> Data { get; set; }  
        public void Dispose()
        {
        }

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            throw new NotImplementedException();
        }
    }

}
