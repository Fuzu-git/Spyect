using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using static Player.DataPlayer.ProfileFiller;

namespace Player.DataPlayer
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public static PlayerBehaviour local;
        
        //[SyncVar]
        public static bool canMove = true;
        
        
        private IEnumerator Start()
        {
            if (isLocalPlayer)
            {
                local = this; 
            
                GameManager.playerList.Add(gameObject);
            }

            if (GameManager.instance)
            {
                GameManager.instance.AddPlayer(this.gameObject);
                canMove = false;
                yield return new WaitForSeconds(5);
                canMove = true; 
            }
            SelectRandomProfile();
        }
        
        private int _profileIndex;
        private readonly SyncList<int> playerIndex = new SyncList<int>();
        public List<PlayerData> profilSo;
        public TMP_Text playerNameText;
        private string _playerInGameName;
        
        void SelectRandomProfile()
        {
            foreach (GameObject player in GameManager.playerList)
            {
                _profileIndex = playerIndex[Random.Range(0, playerIndex.Count)];
            
                NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();
                playerIndex.RemoveAt(_profileIndex);
                
                Reveil(playerIdentity.netId);
            }
        }
        
        void Reveil(uint playerID)
        {
            GameObject myPlayer = NetworkHelper.GetGameObjectFromNetIdValue(playerID, false);
            playerNameText = myPlayer.GetComponent<TMP_Text>();
            
            
            PlayerData currentSo = profilSo[_profileIndex];
            _playerInGameName = currentSo.playerInGameName;
            playerNameText.text = _playerInGameName;
            AssignDataToPlayer();
        }

        [ClientRpc]
        void AssignDataToPlayer()
        {
            PlayerData currentSo = profilSo[_profileIndex];
            _playerInGameName = currentSo.playerInGameName;
            playerNameText.text = _playerInGameName;
        }

    }
}
