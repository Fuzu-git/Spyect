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