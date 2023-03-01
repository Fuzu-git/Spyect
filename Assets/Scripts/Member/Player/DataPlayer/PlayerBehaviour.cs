using System.Collections;
using Mirror;
using UnityEngine;

namespace Member.Player.DataPlayer
{
    public class PlayerBehaviour : AvatarBehaviour
    {
        public static PlayerBehaviour local;

        //public GameObject profileFillerPrefab;
        //private ProfileFiller _profileFillerComponent;

        public GameObject sendVoteUIPrefab;
        [SerializeField] private GameObject instantiatedSendVoteUI;
        public GameObject receiveVoteUIPrefab;
        [SerializeField] private GameObject instantiatedReceiveVoteUI;
        
        protected override IEnumerator Start()
        {
            if (isServer)
            {
                local = this;
            }
            else if (hasAuthority)
            {
                local = this;
            }
            
            if (isServer)
            {
                if(hasAuthority)
                { 
                    //InstantiateProfileFiller();
                    
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
        
        /*[Server]
        void InstantiateProfileFiller()
        {
            GameObject instantiatedProfileFiller = Instantiate(profileFillerPrefab);
            NetworkServer.Spawn(instantiatedProfileFiller);
        }*/
        
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
        
        protected override void OnPlayerStateChanged(PlayerState oldState, PlayerState newState)
        {
            switch (newState)
            {
                case PlayerState.Spectate:
                    gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    //joueur désigné mort. (lien vers UI)
                    break; 
            }
        }
        
        [Command]
        public void CmdVote(PlayerBehaviour playerVoted, EVoteResult voteResult)
        {
            if (GameManager.instance.Vote(this, playerVoted, voteResult))
            {
                GameManager.instance.CheckAllVotes();
            }
        }
    }
}