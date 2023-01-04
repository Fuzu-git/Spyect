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
        
        public SyncList<int> playerIndex = new SyncList<int>();

        private void Awake()
        {
            for (int i = 0; i < profiles.Count ; i++)
            {
                playerIndex.Add(i);
            }
            Shuffle(playerIndex);
        }
        
        public void Shuffle(SyncList<int> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = Random.Range(0, n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}