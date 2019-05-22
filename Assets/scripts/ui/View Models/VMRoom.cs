using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMRoom : GenericSingletonClass<VMRoom>, INotifyPropertyChanged
    {
        public ObservableList<VMRoomItem> items = new ObservableList<VMRoomItem>();

        [Binding]
        public ObservableList<VMRoomItem> Items
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

        public VMRoomItemPosition FindItemByName(string n)
        {
            int j = 0;
            foreach (VMRoomItem i in Items)
            {
                if (i.DisplayText.ToLower() == n.ToLower())
                {
                    VMRoomItemPosition p = new VMRoomItemPosition();
                    p.item = i;
                    p.position = j;

                    return p;
                }

                j++;
            }

            return null;
        }

        public class VMRoomItemPosition
        {
            public VMRoomItem item;
            public int position;
        }
    }
}