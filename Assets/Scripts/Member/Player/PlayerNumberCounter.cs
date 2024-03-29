using Mirror;
using System.Collections.Generic;
using System.Diagnostics;

namespace Member.Player
{
    public class PlayerNumberCounter : NetworkBehaviour
    {
        [SyncVar]
        public int playerNumber = 0;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void CountPlayer(int i)
        {
            playerNumber = i;
        }
    }
}
