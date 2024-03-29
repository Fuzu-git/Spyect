using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lobby;
using Member.AI;
using Member.Player;
using Member.Player.DataPlayer;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Member.SpawnMember
{
    public class MemberSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject aiPrefab;
        private static List<Transform> _spawnPoints = new List<Transform>();

        private int _nextIndex = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            _spawnPoints.Add(transform);
            _spawnPoints = _spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }

        public static void RemoveSpawnPoint(Transform transform) => _spawnPoints.Remove(transform);

        public override void OnStartServer()
        {
            //Shuffle spawnpoints. 
            Shuffle(_spawnPoints);
            
            NetworkManagerLobby.OnServerReadied += SpawnPlayer;
            //NetworkManagerLobby.OnServerReadied += SpawnAi; 
        }

        [ServerCallback]
        private void OnDestroy()
        {
            NetworkManagerLobby.OnServerReadied -= SpawnPlayer;
            //NetworkManagerLobby.OnServerReadied -= SpawnAi; 
        }

        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            Transform spawnPoint = _spawnPoints.ElementAtOrDefault(conn.connectionId);

            if (spawnPoint == null)
            {
                Debug.LogError("Missing spawn point for player " + conn.connectionId);
                return;
            }

            GameObject playerInstance = Instantiate(playerPrefab, _spawnPoints[conn.connectionId].position,
                _spawnPoints[conn.connectionId].rotation);
            NetworkServer.Spawn(playerInstance, conn);
            NetworkServer.ReplacePlayerForConnection(conn.identity.connectionToClient, playerInstance);

            PlayerBehaviour playerBehaviour = playerInstance.GetComponent<PlayerBehaviour>();
            playerBehaviour.SetPlayerIndex(conn.connectionId);
            
            //playerBehaviour.realPlayerName = GameManager.instance.GetNetworkGamePlayer(conn.connectionId).GetDisplayName();
            
            _nextIndex++;
            StartCoroutine(SpawnAi(playerBehaviour));
        }

        [Server]
        public IEnumerator SpawnAi(PlayerBehaviour playerBehaviour)
        {
            while (playerBehaviour.playerIndex == -1)
            {
                yield return null;
            }
            int totalPlayerNumber = NetworkServer.connections.Count();

            InstantiateAI(playerBehaviour.playerIndex+totalPlayerNumber);
        }

        [Server]
        public void InstantiateAI(int aiIndex )
        {
            GameObject aiInstance = Instantiate(aiPrefab, _spawnPoints[aiIndex].position, _spawnPoints[aiIndex].rotation);
            //NetworkServer.Spawn(aiInstance);
            NetworkServer.Spawn(aiInstance, NetworkServer.connections[0]);
            var aiBehaviourInstance = aiInstance.GetComponent<AIBehaviour>();
            aiBehaviourInstance.SetAiIndex(aiIndex);
            /*aiBehaviourInstance.netIdentity.RemoveClientAuthority();
            aiBehaviourInstance.netIdentity.AssignClientAuthority(connectionToClient);*/
            _spawnPoints.RemoveAt(aiIndex);
        }
        public void Shuffle(List<Transform> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}