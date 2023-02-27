using System.Collections.Generic;
using Mirror;
using Random = UnityEngine.Random;

namespace Member.Player.DataPlayer
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
            Shuffle(playerIndex);
        }

        public int GetIndex(int connId)
        {
            return playerIndex[connId];
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