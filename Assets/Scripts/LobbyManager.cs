using UnityEngine;
using UnityEngine.UI;
using Mirror; 
public class LobbyManager : MonoBehaviour
{
    public Button startGameButton; 
    
    void Update()
    {
        if (NetworkServer.active)
        {
            startGameButton.gameObject.SetActive(true); 
            startGameButton.onClick.AddListener(StartGameClicked);
        }
        else
        {
            startGameButton.gameObject.SetActive(false);
        }
    }

    private void StartGameClicked()
    {
        NetworkManager.singleton.ServerChangeScene("Game");
    }
}
