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
        public GameObject instantiatedProfileFiller;

        private ProfileFiller _profileFillerComponent; 
        private IEnumerator Start()
        {
            if (isLocalPlayer)
            {
                local = this; 
            
            }
            
            if (isServer)
            {
                if(hasAuthority)
                { 
                    InstantiateProfileFiller();
                }
                StartCoroutine(SelectRandomProfile());
            }
            
            if (GameManager.instance)
            {
                GameManager.instance.AddPlayer(this.gameObject);
                canMove = false;
                yield return new WaitForSeconds(5);
                canMove = true; 
            }

            
        }

        private int _profileIndex = -1;
        public TMP_Text playerNameText;
        private string _playerInGameName;

        IEnumerator SelectRandomProfile()
        {
            while (instantiatedProfileFiller == null)
            {
                yield return new WaitForSeconds(0.1f); 
                instantiatedProfileFiller = GameObject.FindGameObjectWithTag("profileFiller");
            }

            ProfileFiller profileFillerComponent = instantiatedProfileFiller.GetComponent<ProfileFiller>();

           
            yield return new WaitForSeconds(0.1f);
            
            _profileIndex = profileFillerComponent.GetIndex(connectionToClient.connectionId);


            SendProfilToClient(_profileIndex);

        }

        [ClientRpc]
        void SendProfilToClient(int profileIndex)
        {
            if (_profileFillerComponent == null)
            {
                instantiatedProfileFiller = GameObject.FindGameObjectWithTag("profileFiller");
                _profileFillerComponent = instantiatedProfileFiller.GetComponent<ProfileFiller>();
            }
            PlayerData currentSo = _profileFillerComponent.profiles[profileIndex];
            _playerInGameName = currentSo.playerInGameName;
            playerNameText.text = _playerInGameName;
        }
        
        [Server]
        void InstantiateProfileFiller()
        {
            GameObject instantiatedProfileFiller = Instantiate(profileFillerPrefab);
            NetworkServer.Spawn(instantiatedProfileFiller);
        }
    }
}
