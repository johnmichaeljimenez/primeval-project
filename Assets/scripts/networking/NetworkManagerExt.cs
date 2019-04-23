using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Primeval.Networking
{
    public class NetworkManagerExt : NetworkManager
    {
        public void OnStartLevel()
        {
            print("start level");
            GameManager.instance.StartGame();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            print("OnClientConnect: " + conn.connectionId);
            base.OnClientConnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            print("OnClientDisconnect: " + conn.connectionId);
            base.OnClientDisconnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage extraMessage)
        {
            base.OnServerAddPlayer(conn, extraMessage);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            base.OnClientError(conn, errorCode);
        }
    } 
}
