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
                Debug.Log("ReceiveVoteUI is null");
                yield return new WaitForSeconds(1f);
                _receiveVoteUI = mainCanvas.GetComponentInChildren<ReceiveVoteUI>(true);
            }
        }

        public void FillComponent(int avatarIndex)
        {
            sendVoteUI = GetComponentInParent<SendVoteUI>().transform;
            playerInGameName.text =
                GameManager.instance.memberList[avatarIndex].GetComponent<AvatarBehaviour>().playerNameText.text;
            //PLAYER IMAGE TO DO 
            _avatarIndex = avatarIndex;
            playerSuspected.onClick.AddListener(SuspectedButtonClicked);
        }

        private void SuspectedButtonClicked()
        {
            StartCoroutine(SuspectButtonClickedCo());
        }

        IEnumerator SuspectButtonClickedCo()
        {
            Debug.Log("XXXXX "+(PlayerBehaviour.local == null)+" "+(_receiveVoteUI == null));
            PlayerBehaviour.local.CmdAssignNetworkAuthority(_receiveVoteUI.GetComponent<NetworkIdentity>());
            yield return new WaitForSeconds(0.2f);
            Debug.Log("TEST 0");
            if (!GameManager.instance.IsAlreadySuspected(_avatarIndex))
            {
                Debug.Log("TEST 1");
                _receiveVoteUI.CmdUpdateContentData(_avatarIndex);
            }
            sendVoteUI.gameObject.SetActive(false);
        }
    }
}