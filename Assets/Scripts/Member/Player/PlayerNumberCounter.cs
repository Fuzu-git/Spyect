using Mirror;
using System.Collections.Generic;

namespace Member.Player
{
    public class PlayerNumberCounter : NetworkBehaviour
    {
        [SyncVar]
        public int playerNumber = 0;
        [SyncVar]
        public List<string> playerNames = new List<string>();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void CountPlayer(int i)
        {
            playerNumber = i;
        }

        public void SetNames(List<Lobby.NetworkRoomPlayer> players)
        {
            foreach (Lobby.NetworkRoomPlayer roomPlayer in players)
            {
                playerNames.Add(roomPlayer.displayName);
            }
        }
    }
}
