using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMInteraction : GenericSingletonClass<VMInteraction>, INotifyPropertyChanged
    {

        public Transform panel;

        private string itemName = "";

        [Binding]
        public string ItemName
        {
            get
            {
                return itemName;
            }
            set
            {
                if (itemName == value)
                {
                    return; // No change.
                }

                itemName = value;

                OnPropertyChanged("ItemName");
            }
        }
        
        private int itemWeight = 0;

        [Binding]
        public int ItemWeight
        {
            get
            {
                return itemWeight;
            }
            set
            {
                if (itemWeight == value)
                {
                    return; // No change.
                }

                itemWeight = value;

                OnPropertyChanged("ItemWeight");
            }
        }

        
        private int itemAmount = 0;

        [Binding]
        public int ItemAmount
        {
            get
            {
                return itemAmount;
            }
            set
            {
                if (itemAmount == value)
                {
                    return; // No change.
                }

                itemAmount = value;

                OnPropertyChanged("ItemAmount");
            }
        }
        
        
        private bool current = false;

        [Binding]
        public bool Current
        {
            get
            {
                return current;
            }
            set
            {
                if (current == value)
                {
                    return; // No change.
                }

                current = value;
                panel.gameObject.SetActive(current);

                OnPropertyChanged("Current");
            }
        }

        public void Set(string i, int w, int a, bool c)
        {
            ItemName = i;
            ItemWeight = w;
            ItemAmount = a;
            Current = c;
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}