using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SendVote : MonoBehaviour
    {
        public GameObject playerContent;  
    
        public TMP_Text playerInGameName;
        public Button suspectedButton;
        public Image playerInGameImage;

        private void Start()
        {
            foreach (GameObject element in GameManager.playerList) /*playerList = future memberList*/
            {
                playerInGameName = null;
                playerInGameImage = null;
            }
        }
    }
}
