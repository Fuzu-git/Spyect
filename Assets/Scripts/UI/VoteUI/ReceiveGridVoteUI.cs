using System;
using Member;
using Member.Player.DataPlayer;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class ReceiveGridVoteUI : MonoBehaviour
    {
        public TMP_Text playerVoteText;
        public Image playerInGameImage;

        public Button notSuspectedButton;
        public Button suspectedButton;
        
        public AvatarBehaviour avatarBinding;

        public Transform voteCountGrid; 
        public GameObject playerVoteShape;
        
        private void Start()
        {
            suspectedButton.onClick.AddListener(VoteYes);
            notSuspectedButton.onClick.AddListener(VoteNo);
        }


        public void FillReceiveContent(int avatarIndex)
        {
            avatarBinding = GameManager.instance.memberList[avatarIndex].GetComponent<AvatarBehaviour>();
            playerVoteText.text = GameManager.instance.ProfileFiller.profiles[avatarBinding.profileIndex].playerInGameName.ToUpper() + " is being suspected, what is your opinion ?";
            playerInGameImage.sprite = GameManager.instance.ProfileFiller.profiles[avatarBinding.profileIndex].playerInGameImage;

            foreach (var player in GameManager.instance.playerList)
            {
                Instantiate(playerVoteShape, voteCountGrid);
            }
        }

        private void VoteYes()
        {
            PlayerBehaviour.local.CmdVote(avatarBinding.GetAvatarIndex(), EVoteResult.Yes);
        }

        private void VoteNo()
        {
            PlayerBehaviour.local.CmdVote(avatarBinding.GetAvatarIndex(), EVoteResult.No);
        }

        private void VoteWhite()
        {
            PlayerBehaviour.local.CmdVote(avatarBinding.GetAvatarIndex(), EVoteResult.White);
        }
    }
}