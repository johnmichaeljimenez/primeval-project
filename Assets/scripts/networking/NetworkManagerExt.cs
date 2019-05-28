using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon;

namespace Primeval.Networking
{
    public class NetworkManagerExt : GenericSingletonClass<NetworkManagerExt>
    {
        public static bool isHosting
        {
            get
            {
                if (!PlayerCharacter.PlayerCharacter.myPlayer)
                    return false;

                return PhotonNetwork.isMasterClient;
            }
        }

        static bool connected;

        void Awake()
        {
            base.Awake();
            PhotonPeer.RegisterType(typeof(Primeval.PlayerCharacter.SyncInventoryItem), (byte)'I', Primeval.PlayerCharacter.SyncInventoryItem.Serialize, Primeval.PlayerCharacter.SyncInventoryItem.Deserialize);
            Connect();
        }

        public void Connect()
        {
            connected = false;
            UIManager.ShowLoading(true);
            print("connecting");
            PhotonNetwork.ConnectUsingSettings("v0.0.1");
        }

        void OnJoinedLobby()
        {
            connected = true;
            UIManager.ShowLoading(false);
            print("joined");
            // PhotonNetwork.JoinRandomRoom();
        }

        void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            connected = false;
            print("failed");
            UIManager.ShowLoading(false);
            UIManager.ShowMessage(cause.ToString(), "ERROR", Connect, true, null, "Reconnect");
        }
        void OnConnectionFail(DisconnectCause cause)
        {
            if (connected)
            {
                print("disconnected");
                UIManager.ShowLoading(false);
                UIManager.ShowMessage("You have been disconnected", "ERROR", Connect, true, null, "Reconnect");
            }
        }

        void OnReceivedRoomListUpdate()
        {
            ViewModels.VMRoom vmRoom = ViewModels.VMRoom.instance;
            vmRoom.items.Clear();
            foreach (RoomInfo i in PhotonNetwork.GetRoomList())
            {
                ViewModels.VMRoomItem vmRoomItem = new ViewModels.VMRoomItem(i.Name, i.PlayerCount, i.MaxPlayers, i.Name);
                vmRoom.items.Add(vmRoomItem);
            }
        }

        void OnPhotonRandomJoinFailed()
        {
            Debug.Log("OnPhotonRandomJoinFailed");
            PhotonNetwork.CreateRoom(null);
        }

        void OnJoinedRoom()
        {
            UIManager.ShowLoading(false);
            print("room");
            // PhotonNetwork.Instantiate("Player Character", Vector3.zero, Quaternion.identity, 0);
            // if (PhotonNetwork.isMasterClient)
            //     OnStartLevel();
        }


        public void OnStartLevel()
        {
            print("start level");
            GameManager.instance.StartGame();
        }
        public static void CreateRoom()
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 5;
            options.IsOpen = true;
            PhotonNetwork.CreateRoom("test", options, null);
            UIManager.ShowLoading(true);
        }

        public static void JoinRoom(string n)
        {
            UIManager.ShowLoading(true);
            PhotonNetwork.JoinRoom(n);
        }
    }
}
