using System;
using System.Collections;
using System.Linq;
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
                local = this;
            }
            yield return StartCoroutine(base.Start());
        }

        private void Update()
        {
            Flip(cc.velocity.x);
            float characterVelocityX = Mathf.Abs(cc.velocity.x);
            float characterVelocityY = Mathf.Abs(cc.velocity.y);
            animator.SetFloat("speedX", characterVelocityX + characterVelocityY);
        }

        void UpdatePlayerIndex(int oldValue, int newValue)
        {
            StartCoroutine(WaitForProfiller());
        }

        [Command]
        private void CmdSetPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public override void SelectRandomProfile()
        {
            if (playerIndex == -1 || ProfileFillerComponent == null) return;
            profileIndex = ProfileFillerComponent.GetIndex(playerIndex);
            SendProfilToClient(profileIndex);
        }
        
        public void SetPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
            StartCoroutine(WaitForProfiller());
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
        public void CmdVote(int targetedAvatarIndex, EVoteResult voteResult)
        {
            //recuperer le player/ia en fonction de avatar
            AvatarBehaviour target = GameManager.instance.memberList[targetedAvatarIndex].GetComponent<AvatarBehaviour>();
            if (GameManager.instance.Vote(this, target, voteResult))
            {
                GameManager.instance.CheckAllVotes();
            }
        }

        public override int GetAvatarIndex()
        {
            return playerIndex;
        }
    }
}