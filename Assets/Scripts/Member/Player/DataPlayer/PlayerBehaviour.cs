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
        [SyncVar]
        public int playerIndex = -1;
        
        
        protected override IEnumerator Start()
        {
            if (isServer)
            {
                playerIndex = connectionToClient.connectionId;
            }
            if (isLocalPlayer)
            {
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

        //[Command]
        protected override void CmdSelectRandomProfile()
        {
            profileIndex = ProfileFillerComponent.GetIndex(playerIndex);
            RpcSendProfilToClient(profileIndex);
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