using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class OpenSendVoteUI : MonoBehaviour
    {
        public SendVoteUI sendVoteUI;
        [SerializeField] private Button leaderButton;

        [SerializeField] private Canvas mainCanvas;

        public IEnumerator Start()
        {
            while (sendVoteUI == null)
            {
                Debug.Log("SendVoteUI is null");
                yield return new WaitForSeconds(1f);
                sendVoteUI = mainCanvas.GetComponentInChildren<SendVoteUI>(true);
            }
            leaderButton.onClick.AddListener(OpenUI);
        }

        private void OpenUI()
        {
            sendVoteUI.UpdateGrid();
        }
    }
}