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

        void Awake()
        {
            base.Awake();
            PhotonPeer.RegisterType(typeof(Primeval.PlayerCharacter.SyncInventoryItem), (byte)'I', Primeval.PlayerCharacter.SyncInventoryItem.Serialize, Primeval.PlayerCharacter.SyncInventoryItem.Deserialize);

            UIManager.ShowLoading(true);
            print("connecting");
            PhotonNetwork.ConnectUsingSettings("v0.0.1");
        }

        void OnJoinedLobby()
        {
            UIManager.ShowLoading(false);
            print("joined");
            // PhotonNetwork.JoinRandomRoom();
        }

        void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            UIManager.ShowLoading(false);
            UIManager.ShowMessage("ERROR: " + cause.ToString());
        }
        public virtual void OnDisconnectedFromPhoton()
        {
            UIManager.ShowLoading(false);
            UIManager.ShowMessage("Disconnected");
        }
        
        public virtual void OnReceivedRoomListUpdate()
        {
        }

        void OnPhotonRandomJoinFailed()
        {
            Debug.Log("OnPhotonRandomJoinFailed");
            PhotonNetwork.CreateRoom(null);
        }

        void OnJoinedRoom()
        {
            print("room");

            PhotonNetwork.Instantiate("Player Character", Vector3.zero, Quaternion.identity, 0);
            if (PhotonNetwork.isMasterClient)
                OnStartLevel();
        }


        public void OnStartLevel()
        {
            print("start level");
            GameManager.instance.StartGame();
        }
    }
}
