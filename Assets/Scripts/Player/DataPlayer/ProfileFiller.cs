using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.DataPlayer
{
    public class ProfileFiller : NetworkBehaviour
    {
        public List<PlayerData> profiles = new List<PlayerData>();

        public readonly SyncList<int> playerIndex = new SyncList<int>();
        
        public override void OnStartServer()
        { 
            base.OnStartServer();
            for (int i = 0; i < profiles.Count; i++)
            {
                playerIndex.Add(i);
            }
            Debug.Log("Avant shuffle" + playerIndex.Count);
            Shuffle(playerIndex);
            Debug.Log("Après shuffle" + playerIndex.Count);
            Debug.Log("Start server " + gameObject.name);
        }

        public int GetIndex(int index)
        {
            Debug.Log("Dans index" + playerIndex.Count);
            Debug.Log("Index " + gameObject.name);
            return playerIndex[index];
            
        }

        private void Update()
        {
                Debug.Log("Update " +playerIndex.Count);
        }

        public void Shuffle(SyncList<int> list)
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