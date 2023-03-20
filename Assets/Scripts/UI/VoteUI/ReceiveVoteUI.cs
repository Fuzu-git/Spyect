using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class ReceiveVoteUI : NetworkBehaviour
    {
        [SerializeField] public List<ReceiveGridVoteUI> receiveVoteContentList = new List<ReceiveGridVoteUI>();
        public ReceiveGridVoteUI playerEntryPrefab; 
        public GridLayoutGroup gridLayout;

        public RectTransform mainCanvas;

        private void Start()
        {
            mainCanvas = (RectTransform) GameObject.FindGameObjectWithTag("CanvasUI").transform;
            transform.SetParent(mainCanvas, false);
            RectTransform myTransform = (RectTransform)transform;
            myTransform.anchoredPosition = Vector2.zero;
        }
        
        [Command]
        public void CmdUpdateContentData(int avatarIndex)
        {
             RpcUpdateContentData(avatarIndex);
             Debug.Log("Command Called");
        }

        [ClientRpc]
        private void RpcUpdateContentData(int avatarIndex)
        {
            Debug.Log("ClientRpc called");
            gameObject.SetActive(true);
            ReceiveGridVoteUI receiveGridVoteUi = Instantiate(playerEntryPrefab, gridLayout.transform);
            receiveGridVoteUi.FillReceiveContent(avatarIndex);
            receiveVoteContentList.Add(receiveGridVoteUi);
        }
    }
}
