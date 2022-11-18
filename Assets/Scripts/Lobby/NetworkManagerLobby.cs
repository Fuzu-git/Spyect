using UnityEngine;
using Mirror;
using System; 
using System.Linq; 
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Lobby
{
    public class NetworkManagerLobby : NetworkManager
    {
        [SerializeField] private int minPlayers = 4;
        [Scene] [SerializeField] private string menuScene = string.Empty;
        
        [Header("Room")]
        [SerializeField] private NetworkRoomPlayer roomPlayerPrefab;
        
        [Header("Game")]
        [SerializeField] private NetworkGamePlayer gamePlayerPrefab;
        [SerializeField] private GameObject playerSpawnSystem = null; 

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;

        public List<NetworkRoomPlayer> RoomPlayers { get; } = new();
        public List<NetworkGamePlayer> GamePlayers { get; } = new();

        public override void OnStartServer()
        {
            var spawnPrefabs = Resources.LoadAll<GameObject>("LobbyPrefabs").ToList();
        }
        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
            foreach (var prefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
        }

        [Obsolete("Remove the NetworkConnection parameter in your override and use NetworkClient.connection instead.")]
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }

        [Obsolete("Remove the NetworkConnection parameter in your override and use NetworkClient.connection instead.")]
        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }

            if (SceneManager.GetActiveScene().path != menuScene)
            {
                conn.Disconnect();
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                bool isHost = RoomPlayers.Count == 0;
                
                NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);
                roomPlayerInstance.IsHost = isHost;
                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayer>();
                RoomPlayers.Remove(player);

                NotifyPlayersReady();
            }
            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
        }

        public void NotifyPlayersReady()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        private bool IsReadyToStart()
        {
            if (numPlayers < minPlayers)
            {
                return false; 
            }

            foreach (var player in RoomPlayers)
            {
                if (!player.isReady)
                {
                    return false; 
                }
            }
            return true; 
        }

        public void StartGame()
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                if (!IsReadyToStart())
                {
                    return; 
                }
                ServerChangeScene("Scene_Map_01"); 
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Scene_Map_01"))
            {
                for (int i = RoomPlayers.Count - 1; i > 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetDisplayName(RoomPlayers[i].displayName);
                    
                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
                }
            }
            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.StartsWith("Scene_Map_"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
            }
        }
        
        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            OnServerReadied?.Invoke(conn);
        }
    }
}
