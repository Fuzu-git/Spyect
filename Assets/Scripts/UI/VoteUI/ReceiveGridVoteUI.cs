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
        public TMP_Text playerNameVoteText;
        public Image playerInGameImage;

        public Button notSuspectedButton;
        public Button suspectedButton;

        public AvatarBehaviour avatarBinding; 

        private void Start()
        {
            suspectedButton.onClick.AddListener(VoteYes);
            notSuspectedButton.onClick.AddListener(VoteNo);
        }


        public void FillReceiveContent(int avatarIndex)
        {
            avatarBinding = GameManager.instance.memberList[avatarIndex].GetComponent<AvatarBehaviour>();
            playerNameVoteText.text = GameManager.instance.ProfileFiller.profiles[avatarBinding.profileIndex].playerInGameName;
            playerInGameImage.sprite = GameManager.instance.ProfileFiller.profiles[avatarBinding.profileIndex].playerInGameImage;
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