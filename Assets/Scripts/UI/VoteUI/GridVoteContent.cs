using System;
using System.Collections;
using System.Collections.Generic;
using Member;
using Member.Player.DataPlayer;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class GridVoteContent : MonoBehaviour
    {
        public TMP_Text playerInGameName;
        public Image playerInGameAvatar;

        public Button playerSuspected;

        public Transform sendVoteUI;

        //private string _playerInGameNameText;

        //private new List<String> _playerNamesList = new List<string>();

        private int _avatarIndex;

        public ReceiveVoteUI _receiveVoteUI;

        public RectTransform mainCanvas;

        public IEnumerator Start()
        {
            mainCanvas = GameObject.FindGameObjectWithTag("CanvasUI").GetComponent<RectTransform>();

            while (_receiveVoteUI == null)
            {
                yield return new WaitForSeconds(1f);
                _receiveVoteUI = mainCanvas.GetComponentInChildren<ReceiveVoteUI>(true);
            }
        }

        public void FillComponent(int avatarIndex)
        {
            sendVoteUI = GetComponentInParent<SendVoteUI>().transform;
            AvatarBehaviour avatar =GameManager.instance.memberList[avatarIndex].GetComponent<AvatarBehaviour>();
            playerInGameName.text = GameManager.instance.ProfileFiller.profiles[avatar.profileIndex].playerInGameName;
            playerInGameAvatar.sprite = GameManager.instance.ProfileFiller.profiles[avatar.profileIndex].playerInGameImage;
            _avatarIndex = avatarIndex;
            playerSuspected.onClick.AddListener(SuspectedButtonClicked);
        }

        private void SuspectedButtonClicked()
        {
            StartCoroutine(SuspectButtonClickedCo());
        }

        IEnumerator SuspectButtonClickedCo()
        {
            PlayerBehaviour.local.CmdAssignNetworkAuthority(_receiveVoteUI.GetComponent<NetworkIdentity>());
            yield return new WaitForSeconds(0.2f);
            if (!GameManager.instance.IsAlreadySuspected(_avatarIndex))
            {
                _receiveVoteUI.CmdUpdateContentData(_avatarIndex);
            }
            sendVoteUI.gameObject.SetActive(false);
        }
    }
}