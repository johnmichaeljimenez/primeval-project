using UnityWeld.Binding;

/// <summary>
/// View-model for items displayed in the collection.
/// </summary>

namespace Primeval.ViewModels
{
    [Binding]
    public class VMInventoryItem
    {
        public string itemName;

        [Binding]
        public string DisplayText
        {
            get;
            private set;
        }

        [Binding]
        public string Hotkey
        {
            get;
            private set;
        }
        

        [Binding]
        public int Amount
        {
            get;
            private set;
        }
        

        [Binding]
        public int TotalWeight
        {
            get;
            private set;
        }

        public VMInventoryItem(string displayText, int amount, int totalWeight, Data.KeyBindings hotkey)
        {
            itemName = displayText;
            this.DisplayText = displayText;
            this.Amount = amount;
            this.TotalWeight = totalWeight;

            string hk = "";
            int hk2 = (int)hotkey;

            if (hk2 > 0)
                hk = (hk2-48).ToString(); //TODO: change this

            this.Hotkey = hk;
        }
    }

}