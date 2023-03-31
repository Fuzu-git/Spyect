using System;
using System.Collections;
using System.Collections.Generic;
using Lobby;
using Member;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class SendVoteUI : NetworkBehaviour
    {
        public GameObject gridContent;
        public GridLayoutGroup gridLayout;

        public GridVoteContent playerEntryPrefab;
        [SerializeField] private List<GridVoteContent> gridVoteContentList = new List<GridVoteContent>();

        public RectTransform mainCanvasUI;

        private bool once = false;

        private IEnumerator Start()
        {
            if (once == false)
            {
                /*int totalPlayerNumber = GameObject.FindGameObjectWithTag("PlayerCounter")
                    .GetComponent<PlayerNumberCounter>().PlayerNumber;

                while (GameManager.playerList.Count != totalPlayerNumber)
                {
                    yield return new WaitForSeconds(1f);
                }*/

                while (mainCanvasUI == null)
                {
                    GameObject temp = GameObject.FindGameObjectWithTag("CanvasUI");
                    if (temp != null)
                    {
                        mainCanvasUI = (RectTransform)temp.transform;
                    }
                    yield return new WaitForSeconds(1f);
                }
                
                transform.SetParent(mainCanvasUI, false);
                RectTransform myTransform = (RectTransform) transform;
                myTransform.anchoredPosition = Vector2.zero;


                gameObject.SetActive(false);
                once = true;
            }


            /*foreach (var player in GameManager.playerList)
            {
                playerEntryPrefab._playerInGameName = player.GetComponent<PlayerBehaviour>().playerNameText;
                gridVoteContentList.Add(playerEntryPrefab);
            }*/
        }

        /// <summary>
        /// grid of people you can accuse
        /// </summary>
        public void UpdateGrid()
        {
            gameObject.SetActive(true);
            if (gridVoteContentList.Count > 0)
            {
                foreach (GridVoteContent content in gridVoteContentList)
                {
                    Destroy(content.gameObject);
                }

                gridVoteContentList.Clear();
            }

            for (int i = 0; i < GameManager.instance.memberList.Count; i++)
            {
                if (GameManager.instance.memberList[i].GetComponent<AvatarBehaviour>().State == PlayerState.Alive)
                { 
                    GridVoteContent go = Instantiate(playerEntryPrefab, gridLayout.transform);
                    go.FillComponent(i); 
                    gridVoteContentList.Add(go);
                }
            }
        }
    }
}