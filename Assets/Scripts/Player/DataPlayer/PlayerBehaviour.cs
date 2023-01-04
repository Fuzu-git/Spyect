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
        
        public GameObject profileFillerPrefab;
        public GameObject instantiatedProfileFIller; 
        
        private IEnumerator Start()
        {
            if (isLocalPlayer)
            {
                local = this; 
            
                //GameManager.playerList.Add(gameObject);
            }

            
            if (GameManager.instance)
            {
                GameManager.instance.AddPlayer(this.gameObject);
                canMove = false;
                yield return new WaitForSeconds(5);
                canMove = true; 
            }

            if (isServer && hasAuthority)
            {
                Debug.Log("isLocalPlayer Entry");
                InstantiateProfileFiller();
            }
            StartCoroutine(SelectRandomProfile());
        }
        
        private int _profileIndex = -1;
        public TMP_Text playerNameText;
        private string _playerInGameName;

        IEnumerator SelectRandomProfile()
        {
            while (instantiatedProfileFIller == null)
            {
                yield return new WaitForSeconds(0.1f); 
                instantiatedProfileFIller = GameObject.FindGameObjectWithTag("profileFiller");
            }
            
            Debug.Log(connectionToClient.connectionId);
            
            ProfileFiller profileFillerComponent = instantiatedProfileFIller.GetComponent<ProfileFiller>();

           
            yield return new WaitForSeconds(0.1f);
            
            _profileIndex = profileFillerComponent.GetIndex(connectionToClient.connectionId);

            Debug.Log(_profileIndex);
            
            PlayerData currentSo = profileFillerComponent.profiles[_profileIndex];
            _playerInGameName = currentSo.playerInGameName;
            playerNameText.text = _playerInGameName;
            
        }


        [Server]
        void InstantiateProfileFiller()
        {
            Debug.Log("Entry instantiate");
            GameObject instantiatedProfileFiller = Instantiate(profileFillerPrefab);
            NetworkServer.Spawn(instantiatedProfileFiller);
        }
    }
}
