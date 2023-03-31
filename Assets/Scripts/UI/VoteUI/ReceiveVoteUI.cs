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
            RectTransform myTransform = (RectTransform) transform;
            myTransform.anchoredPosition = Vector2.zero;
        }

        [Command]
        public void CmdUpdateContentData(int avatarIndex)
        {
            RpcUpdateContentData(avatarIndex);
        }

        [ClientRpc]
        private void RpcUpdateContentData(int avatarIndex)
        {
            gameObject.SetActive(true);
            ReceiveGridVoteUI receiveGridVoteUi = Instantiate(playerEntryPrefab, gridLayout.transform);
            receiveGridVoteUi.FillReceiveContent(avatarIndex);
            receiveVoteContentList.Add(receiveGridVoteUi);
        }

        [ClientRpc]
        public void RpcColorFirstColorlessShape(Color voteColor, int avatarIndex)
        {
            foreach (ReceiveGridVoteUI vote in receiveVoteContentList)
            {
                if (vote.avatarBinding.GetAvatarIndex() == avatarIndex)
                {

                    foreach (var shape in vote.playerVoteShapeList)
                    {

                        if (shape.color == Color.white)
                        {

                            shape.color = voteColor;
                            return;
                        }
                    }
                }
            }
        }

        [ClientRpc]
        public void RpcCloseVoteContent(int avatarIndex)
        {
            for (int i = receiveVoteContentList.Count - 1; i >= 0; i--)
            {
                var vote = receiveVoteContentList[i];

                if (vote.avatarBinding.GetAvatarIndex() == avatarIndex)
                {
                    receiveVoteContentList.RemoveAt(i);
                    Destroy(vote.gameObject);
                }
            }
        }
    }
}