using UnityEngine;

namespace Lobby
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager;

        [SerializeField] private GameObject landingPagePanel; 
        
        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }
    }
}
