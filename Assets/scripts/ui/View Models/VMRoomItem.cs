using UnityWeld.Binding;

/// <summary>
/// View-model for items displayed in the collection.
/// </summary>

namespace Primeval.ViewModels
{
    [Binding]
    public class VMRoomItem
    {
        public string displayText;

        [Binding]
        public string DisplayText
        {
            get;
            private set;
        }

        public VMRoomItem(string t)
        {
            displayText = t;
            this.DisplayText = displayText;
        }
    }

}