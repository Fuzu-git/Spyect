using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Ranking
{
    public class RankingMessageManager : MonoBehaviour
    {
        
        [SerializeField]
        private TMP_Text playerInGameName;
        
        [SerializeField]
        private TMP_Text playerNumberRank;
        [SerializeField]
        private Image playerImage;

        public void FillDataMessage(string playerName, string playerInGameName, string playerNumberRank, Sprite playerImage)
        {
            this.playerInGameName.text = $"{playerName} \n ({playerInGameName})";
            this.playerNumberRank.text = playerNumberRank;
            this.playerImage.sprite = playerImage;
            
        }
    }
}
