using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class JoinMenuLobby : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager;

        [SerializeField] private GameObject landingPagePanel;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private Button joinButton;

        private void OnEnable()
        {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAddress = ipAddressInputField.text;

            networkManager.networkAddress = ipAddress;
            networkManager.StartClient();

            joinButton.interactable = false; 
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true; 
        
            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true; 
        }
    }
}
