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
        public string Key { get; set; } = "key";
        public ObservableCollection<int> Data { get; set; } = new ObservableCollection<int>() {1,2,3,4,5,6,7};
        public void Dispose()
        {
        }

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            throw new NotImplementedException();
        }
    }

}
