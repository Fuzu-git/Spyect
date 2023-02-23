using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using static Player.DataPlayer.ProfileFiller;

namespace Player.DataPlayer
{
    public enum PlayerState
    {
        Alive, 
        Spectate
    }
    
    public class PlayerBehaviour : NetworkBehaviour
    {
        public static PlayerBehaviour local;

        [SyncVar(hook = nameof(OnPlayerStateChanged))] 
        protected internal PlayerState state = PlayerState.Alive; 
        public static bool canMove = true;
        
        public GameObject profileFillerPrefab;
        public GameObject instantiatedProfileFiller;

        private ProfileFiller _profileFillerComponent;

        public GameObject sendVoteUIPrefab;
        [SerializeField] private GameObject instantiatedSendVoteUI;

        public GameObject receiveVoteUIPrefab;
        [SerializeField] private GameObject instantiatedReceiveVoteUI;
        
        private IEnumerator Start()
        {
            Debug.Log("START");
            if (isServer)
            {
                local = this;
                gameObject.name = "SERVER";
                Debug.Log("LOCAL SERVER");
            }
            else if (hasAuthority)
            {
                local = this;
                gameObject.name = "CLIENT";
                Debug.Log("LOCAL CLIENT");
            }
            
            if (isServer)
            {
                if(hasAuthority)
                { 
                    InstantiateProfileFiller();
                    
                    InstantiateGridVote();
                    InstantiateReceiveVote();
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
                instantiatedProfileFiller = GameObject.FindGameObjectWithTag("profileFiller");
                yield return new WaitForSeconds(0.1f); 
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
        
        [Server]
        private void InstantiateGridVote()
        {
            instantiatedSendVoteUI = Instantiate(sendVoteUIPrefab);
            NetworkServer.Spawn(instantiatedSendVoteUI);
        }

        [Server]
        private void InstantiateReceiveVote()
        {
            instantiatedReceiveVoteUI = Instantiate(receiveVoteUIPrefab);
            NetworkServer.Spawn(instantiatedReceiveVoteUI);
            instantiatedReceiveVoteUI.GetComponent<NetworkIdentity>().RemoveClientAuthority();
        }
        
        [Command]
        public void CmdAssignNetworkAuthority(NetworkIdentity uiAuthorityId)
        {
            //If -> cube has a owner && owner isn't the actual owner
            if (uiAuthorityId.hasAuthority == false)
            {
                // Remove authority
                uiAuthorityId.RemoveClientAuthority();
            }

            // Add client as owner
            uiAuthorityId.AssignClientAuthority(netIdentity.connectionToClient);
        }

        private void OnPlayerStateChanged(PlayerState oldState, PlayerState newState)
        {
            switch (newState)
            {
                case PlayerState.Spectate:
                    gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    break; 
            }
        }
        
        [Command]
        public void CmdVote(PlayerBehaviour playerVoted)
        {
            if (GameManager.instance.Vote(this, playerVoted))
            {
                GameManager.instance.CheckAllVotes(); 
            }
        }

        [ClientRpc]
        public void RpcVote(PlayerBehaviour playerVoted)
        {
            
        }
        
    }
}
