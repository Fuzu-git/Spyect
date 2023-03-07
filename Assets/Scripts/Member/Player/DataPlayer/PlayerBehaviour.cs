using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Member.Player.DataPlayer
{
    public class PlayerBehaviour : AvatarBehaviour
    {
        public CharacterController cc; 
        public static PlayerBehaviour local;
        [SyncVar (hook = nameof(UpdatePlayerIndex))]
        public int playerIndex = -1;
        
        protected override IEnumerator Start()
        {
            if (isLocalPlayer)
            {
                if (isServer)
                {
                    playerIndex = connectionToClient.connectionId;
                }
                else
                {
                    playerIndex = 1 + connectionToServer.connectionId;
                    CmdSetPlayerIndex(playerIndex);
                }
                local = this;
            }
            yield return StartCoroutine(base.Start());
        }

        private void Update()
        {
            Flip(cc.velocity.x);
            float characterVelocity = Mathf.Abs(cc.velocity.x);
            animator.SetFloat("speed", characterVelocity);
        }

        void UpdatePlayerIndex(int oldValue, int newValue)
        {
            Debug.Log("HOOKED "+oldValue+" "+newValue);
            playerIndex = newValue;
            Debug.Log("HOOKED 1");
            SelectRandomProfile();
            Debug.Log("HOOKED 2 "+isLocalPlayer);
        }

        [Command]
        private void CmdSetPlayerIndex(int playerIndex)
        {
            Debug.Log("CMD RECEIVE");
            this.playerIndex = playerIndex;
        }

        //[Command]
        protected override void SelectRandomProfile()
        {
            if (playerIndex == -1) return;
            profileIndex = ProfileFillerComponent.GetIndex(playerIndex);
            SendProfilToClient(profileIndex);
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