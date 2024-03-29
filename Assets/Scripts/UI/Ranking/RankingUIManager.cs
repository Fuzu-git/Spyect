using System;
using System.Collections.Generic;
using System.Linq;
using Member.Player.DataPlayer;
using UnityEngine;

namespace UI.Ranking
{
    public class RankingUIManager : MonoBehaviour
    {
        public GameObject rankingMessagePrefab;
        public List<GameObject> rankingMessageList = new List<GameObject>();
        public Transform rankingGrid;

        private void Awake()
        {
            FillUIRanking();
        }

        private void FillUIRanking()
        {
            for (int i = 0; i < GameManager.instance.playerList.Count; i++)
            {
                rankingMessageList.Add(Instantiate(rankingMessagePrefab, rankingGrid));
            }

            FillRankingMessage();
        }

        public void FillRankingMessage()
        {
            List<PlayerBehaviour> playerBehavioursList = new List<PlayerBehaviour>();
            for (int i = 0; i < GameManager.instance.playerList.Count; i++)
            {
                PlayerBehaviour playerBehaviour = GameManager.instance.playerList[i].GetComponent<PlayerBehaviour>();
                playerBehavioursList.Add(playerBehaviour);
            }

            playerBehavioursList = playerBehavioursList.OrderBy(x => x.playerRank).ToList();

            for (int i = 0; i < playerBehavioursList.Count; i++)
            {
                PlayerBehaviour playerBehaviour = playerBehavioursList[i];
                rankingMessageList[i].GetComponent<RankingMessageManager>().FillDataMessage(
                    GameManager.instance.GetNetworkGamePlayer(playerBehaviour.playerIndex).GetDisplayName(),
                    playerBehaviour.playerNameText.text, playerBehaviour.playerRank.ToString(),
                    GameManager.instance.ProfileFiller.profiles[playerBehaviour.profileIndex].playerInGameImage);
            }
        }
    }
}