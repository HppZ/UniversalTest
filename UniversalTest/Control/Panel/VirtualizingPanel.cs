using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UniversalTest.Control.ListView;

namespace UniversalTest.Control
{
    class VirtualizingPanel : Windows.UI.Xaml.Controls.Panel
    {
        public VirtualizingPanel()
        {
            
        }

        protected override Size MeasureOverride(Size availableSize)
        {

            var itemscontrol = ItemsControl.GetItemsOwner(this);
            var generator = itemscontrol.ItemContainerGenerator;

            return base.MeasureOverride(availableSize);
        }


    }


}
