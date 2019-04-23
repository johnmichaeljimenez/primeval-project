using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMInventory : GenericSingletonClass<VMInventory>, INotifyPropertyChanged
    {
        public ObservableList<VMInventoryItem> items = new ObservableList<VMInventoryItem>();

        [Binding]
        public ObservableList<VMInventoryItem> Items
        {
            get
            {
                return items;
            }
        }


        /// <summary>
        /// Event to raise when a property's value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public VMInventoryItemPosition FindItemByName(string n)
        {
            int j = 0;
            foreach (VMInventoryItem i in Items)
            {
                if (i.itemName.ToLower() == n.ToLower())
                {
                    VMInventoryItemPosition p = new VMInventoryItemPosition();
                    p.item = i;
                    p.position = j;

                    return p;
                }

                j++;
            }

            return null;
        }

        public class VMInventoryItemPosition
        {
            public VMInventoryItem item;
            public int position;
        }
    }
}