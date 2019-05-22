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
            PhotonPeer.RegisterType(typeof(Primeval.PlayerCharacter.SyncInventoryItem), (byte)'W', Primeval.PlayerCharacter.SyncInventoryItem.Serialize, Primeval.PlayerCharacter.SyncInventoryItem.Deserialize);

            print("connecting");
            PhotonNetwork.ConnectUsingSettings("v0.0.1");
        }

        void OnJoinedLobby()
        {
            print("joined");
            PhotonNetwork.JoinRandomRoom();
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

        // public override void OnClientConnect(NetworkConnection conn)
        // {
        //     print("OnClientConnect: " + conn.connectionId);
        //     base.OnClientConnect(conn);
        // }

        // public override void OnClientDisconnect(NetworkConnection conn)
        // {
        //     print("OnClientDisconnect: " + conn.connectionId);
        //     base.OnClientDisconnect(conn);
        // }

        // public override void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage extraMessage)
        // {
        //     base.OnServerAddPlayer(conn, extraMessage);
        // }

        // public override void OnClientError(NetworkConnection conn, int errorCode)
        // {
        //     base.OnClientError(conn, errorCode);
        // }
    }
}
