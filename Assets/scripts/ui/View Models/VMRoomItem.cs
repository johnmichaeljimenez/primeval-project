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
        public string roomName;

        [Binding]
        public string RoomName
        {
            get;
            private set;
        }
        
        public string playerCount;

        [Binding]
        public string PlayerCount
        {
            get;
            private set;
        }

        public VMRoomItem(string t, int pc, int pc2, string rn)
        {
            displayText = t;
            this.DisplayText = displayText;

            playerCount = pc.Padded() + "/" + pc2.Padded();
            this.PlayerCount = playerCount;

            roomName = rn;
            this.RoomName = roomName;
        }
    }

}