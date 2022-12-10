using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

// REPLACE "PLAYER" WITH "MEMBERS"

namespace Player.DataPlayer
{
    public class PlayerAssignScriptable : MonoBehaviour
    {
        private PlayerBehaviour _player = PlayerBehaviour.local;
        
        [SerializeField]
        public List<PlayerData> profilSO;
        private readonly List<GameObject> _playerList = GameManager.playerList;
        //private readonly List<GameObject> _botList = [...];

        public TMP_Text playerInGameName;
        public Sprite playerInGameImage;
        private void Awake()
        {
            List<GameObject> tempList = _playerList;

            foreach (var player in _playerList)
            {
                int currentRandomIndex = Random.Range(0, tempList.Count);
                
                //GET SCRIPTABLE VAR 
                PlayerData currentSO = profilSO[currentRandomIndex];
                string currentSO_Name = currentSO.playerInGameName;
                Sprite currentSO_Image = currentSO.playerInGameImage;
                // END OF GETTING
                
                if (player == _player.gameObject)
                {
                    playerInGameName.text = currentSO_Name;
                    playerInGameImage = currentSO_Image;
                }
                tempList.RemoveAt(currentRandomIndex);
            }
        }
    }
}
