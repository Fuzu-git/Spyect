using System;
using Mirror;
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

        public PlayerBehaviour playerBinding; 

        private void Start()
        {
            suspectedButton.onClick.AddListener(VoteYes);
            notSuspectedButton.onClick.AddListener(VoteNo);
        }


        public void FillReceiveContent(int playerIndex)
        {
            playerNameVoteText.text =
                GameManager.playerList[playerIndex].GetComponent<PlayerBehaviour>().playerNameText.text;
            //Player Image TO DO 
        }

        private void VoteYes()
        {
            PlayerBehaviour.local.CmdVote(playerBinding, EVoteResult.Yes); 
        }

        private void VoteNo()
        {
            PlayerBehaviour.local.CmdVote(playerBinding, EVoteResult.No);
        }

        private void VoteWhite()
        {
            PlayerBehaviour.local.CmdVote(playerBinding, EVoteResult.White);
        }
    }
}