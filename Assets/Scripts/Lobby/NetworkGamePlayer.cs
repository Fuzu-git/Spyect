using System;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

namespace Lobby
{
    public class NetworkGamePlayer : NetworkBehaviour
    {
        [SyncVar]
        private string displayName = "Loading...";
        [SyncVar]
        private int playerIndex = -1;

        private NetworkManagerLobby _room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (_room != null)
                {
                    return _room;
                }
                return _room = NetworkManager.singleton as NetworkManagerLobby; 
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);
            Room.GamePlayers.Add(this);
        }

        public override void OnStopServer()
        {
            Room.GamePlayers.Remove(this);
        }

        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }

        [Server]
        public void SetPlayerIndex(int index)
        {
            this.playerIndex = index;
        }

        public string GetDisplayName()
        {
            return this.displayName; 
        }

        public int GetPlayerIndex()
        {
            return this.playerIndex; 
        }
        
    }
}
