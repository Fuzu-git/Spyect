using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.DataPlayer
{
    public class PlayerData : MonoBehaviour
    {
        private static readonly List<GameObject> TempPlayerList = GameManager.playerList;
        
        public List<string> playerInGameNameList = new List<string>();
        public List<Sprite> playerInGameImageList = new List<Sprite>();

        private readonly PlayerBehaviour _localPlayer = PlayerBehaviour.local;

        public Sprite localPlayerSprite;
        public string localPlayerName;
        
        private void Start()
        {
            foreach (GameObject element in TempPlayerList)
            {
                int currentIndex = Random.Range(0, TempPlayerList.Count);

                if (element.gameObject == _localPlayer.gameObject)
                {
                    localPlayerSprite = playerInGameImageList[currentIndex];
                    localPlayerName = playerInGameNameList[currentIndex];
                }
            }
        }
    }
}
