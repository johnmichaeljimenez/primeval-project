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

        public VMInventoryItem(string displayText, int amount)
        {
            itemName = displayText;
            this.DisplayText = displayText + " [" + amount.ToString() + "]";
        }
    }

}