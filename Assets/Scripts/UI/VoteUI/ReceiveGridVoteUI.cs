using System;
using Player.DataPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class ReceiveGridVoteUI : MonoBehaviour
    {
        public TMP_Text playerNameVoteText;
        public Image playerNameImage;

        public Button notSuspectedButton;
        public Button suspectedButton;

        
        public void FillReceiveContent(int playerIndex)
        {
            playerNameVoteText.text = GameManager.playerList[playerIndex].GetComponent<PlayerBehaviour>().playerNameText.text;
            //Player Image TO DO 
        }
        
    }
}
