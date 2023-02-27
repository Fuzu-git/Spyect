using System.Collections.Generic;
using System.Linq;
using Lobby;
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
        }

        [ServerCallback]
        private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            Transform spawnPoint = _spawnPoints.ElementAtOrDefault(_nextIndex);

            if (spawnPoint == null)
            {
                Debug.LogError("Missing spawn point for player " + _nextIndex);
                return;
            }

            GameObject playerInstance = Instantiate(playerPrefab, _spawnPoints[_nextIndex].position,
                _spawnPoints[_nextIndex].rotation);
            NetworkServer.Spawn(playerInstance, conn);

            SpawnAI(_nextIndex);
            
            _nextIndex++;
        }

        public void SpawnAI(int nextNextIndex)
        {
            nextNextIndex++; 
            GameObject aiInstance = Instantiate(aiPrefab, _spawnPoints[nextNextIndex].position, _spawnPoints[nextNextIndex].rotation);
            NetworkServer.Spawn(aiInstance);
            _spawnPoints.RemoveAt(nextNextIndex);
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