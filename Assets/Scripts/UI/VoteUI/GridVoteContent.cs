using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player.DataPlayer;
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

        private string _playerInGameNameText;

        private new List<String> _playerNamesList = new List<string>();

        private int _playerIndex;

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

        public void FillComponent(int playerIndex)
        {
            sendVoteUI = GetComponentInParent<SendVoteUI>().transform;
            
            playerInGameName.text = GameManager.playerList[playerIndex].GetComponent<PlayerBehaviour>().playerNameText.text;
            //PLAYER IMAGE TO DO 
            _playerIndex = playerIndex;
            playerSuspected.onClick.AddListener(SuspectedButtonClicked);
        }

        private void SuspectedButtonClicked()
        {
            StartCoroutine(SuspectButtonClickedCo());
        }

        IEnumerator SuspectButtonClickedCo()
        {
            Debug.Log("LOCAL " + PlayerBehaviour.local.gameObject.name + " " + (PlayerBehaviour.local != null));
            Debug.Log("XXXXXXXXXXXXXXXXXXXXXXXxx " + (_receiveVoteUI != null));
            PlayerBehaviour.local.CmdAssignNetworkAuthority(_receiveVoteUI.GetComponent<NetworkIdentity>());
            yield return new WaitForSeconds(0.2f);
            _receiveVoteUI.CmdUpdateContentData(_playerIndex);
            sendVoteUI.gameObject.SetActive(false);
        }
    }
}