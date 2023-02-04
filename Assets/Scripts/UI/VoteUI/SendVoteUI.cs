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
    public class SendVoteUI : NetworkBehaviour
    {
        public GameObject gridContent;
        public GridLayoutGroup gridLayout;

        public GridVoteContent playerEntryPrefab;
        [SerializeField] 
        private List<GridVoteContent> gridVoteContentList = new List<GridVoteContent>();

        private IEnumerator Start()
        {
            if (!NetworkServer.active)
            {
                yield return new WaitForSeconds(1f);
            }
            
            for (int i = 0; i < GameManager.playerList.Count; i++)
            {
                gridVoteContentList.Add(playerEntryPrefab);

                foreach (var player in GameManager.playerList)
                {
                    gridVoteContentList[i]._playerInGameName = player.GetComponent<PlayerBehaviour>().playerNameText;

                    Instantiate(gridVoteContentList[i], gridLayout.transform);
                }
            }

            /*foreach (var player in GameManager.playerList)
            {
                playerEntryPrefab._playerInGameName = player.GetComponent<PlayerBehaviour>().playerNameText;
                gridVoteContentList.Add(playerEntryPrefab);
            }*/
        }
    }
}