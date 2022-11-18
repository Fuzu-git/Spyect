using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

namespace Lobby
{
    public class NetworkRoomPlayer : NetworkBehaviour
    {
        [SerializeField] private GameObject lobbyUI;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        [SerializeField] public Button startGameButton; 

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string displayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool isReady;
        
        private bool _isHost;
        public bool IsHost
        {
            set
            {
                _isHost = value;
                startGameButton.gameObject.SetActive(value);
            }
        }

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

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(PlayerNameInput.DisplayName);
            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);
            UpdateDisplay();
        }

        public override void OnStopServer()
        {
            Room.RoomPlayers.Remove(this);
            UpdateDisplay();
        }

        public void HandleReadyStatusChanged(bool oldValue, bool newValue)
        {
            UpdateDisplay();
        }
        public void HandleDisplayNameChanged(string oldValue, string newValue)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (!hasAuthority)
            {
                foreach (var player in Room.RoomPlayers)
                {
                    if (player.hasAuthority)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                return; 
            }
            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting For Player...";
                playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.RoomPlayers[i].displayName;
                playerReadyTexts[i].text = Room.RoomPlayers[i].isReady
                    ? "<color=green>Ready</color>"
                    : "<color=red>Not Ready</color>";
            }
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            if (!_isHost)
            {
                return;
            }
            startGameButton.interactable = readyToStart;
        }

        [Command]
        private void CmdSetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }

        [Command]
        public void CmdReadyUp()
        {
            isReady = !isReady;
            
            Room.NotifyPlayersReady();
        }

        [Command]
        public void CmdStartGame()
        {
            if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
            {
            }
            Room.StartGame();
        }
    }
}
