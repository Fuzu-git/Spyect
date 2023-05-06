using System;
using System.Collections;
using System.Linq;
using Mirror;
using UI.VoteUI;
using UnityEngine;
using UnityEngine.AI;

namespace Member.Player.DataPlayer
{
    public class PlayerBehaviour : AvatarBehaviour
    {
        public static PlayerBehaviour local;
        [SyncVar (hook = nameof(UpdatePlayerIndex))]
        public int playerIndex = -1;

        [SyncVar (hook =  nameof(UpdatePlayerRank))]
        public int playerRank = -1;

        [SyncVar (hook = nameof(UpdateRealPlayerName))]
        public string realPlayerName; 
        
        private Vector3 _lastPosition;
        private Transform _transform;
        
        
        [SerializeField] private NavMeshMovement navMeshMovement;
        
        private void Awake()
        {
            
            _transform = transform;
            _lastPosition = _transform.position;
        }
        
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
            Vector3 velocity = _transform.position - _lastPosition; 
            
            if ((velocity.x != 0 || velocity.z != 0) && canMove)
            {
                Flip(velocity.x);
                animator.SetFloat("speedX",  velocity.x);
            }
            
            _lastPosition = _transform.position;
        }

        void UpdatePlayerIndex(int oldValue, int newValue)
        {
            StartCoroutine(WaitForProfiller());
        }

        void UpdatePlayerRank(int oldValue, int newValue)
        {
            playerRank = newValue;
            GameManager.instance.UpdateRankingUIManager();
        }

        void UpdateRealPlayerName(string oldValue, string newValue)
        {
            realPlayerName = newValue; 
        }

        protected override void SelectRandomProfile()
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
            
            // Remove authority
            uiAuthorityId.RemoveClientAuthority();
            // Add client as owner
            uiAuthorityId.AssignClientAuthority(netIdentity.connectionToClient);
        }
        
        public override void OnPlayerStateChanged(PlayerState oldState, PlayerState newState)
        {
            switch (newState)
            {
                case PlayerState.Dead:
                    //gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    var renderers = gameObject.GetComponentsInChildren<SpriteRenderer>(true);
                    var meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>(true);
                    foreach (var r in renderers)
                    {
                        r.gameObject.layer = 8; //DeadPLayers
                        Color rColor = spriteRenderer.color;
                        rColor.a = 0.39f;
                        r.color = rColor;
                    }
                    foreach (var mr in meshRenderers)
                    {
                        mr.gameObject.layer = 8;
                    }
                    GameManager.CheckVictory();
                    
                    if (isLocalPlayer)
                    {
                        Camera.main.cullingMask = -1;
                    }
                    //Change mainCamera to render DeadPlayers. (can't access culling mask. 
                    //joueur désigné mort. (lien vers UI)
                    break; 
                
                case PlayerState.Spectate:
                    break; 
            }
        }
        
        [Command]
        public void CmdVote(int targetedAvatarIndex, EVoteResult voteResult)
        {
            AvatarBehaviour target = GameManager.instance.memberList[targetedAvatarIndex].GetComponent<AvatarBehaviour>();

            if (GameManager.instance.Vote(this, target, voteResult))
            {
                GameManager.instance.CheckAllVotes(targetedAvatarIndex);
                
                if (voteResult == EVoteResult.Yes)
                {
                    GameManager.instance.ReceiveVoteUI.RpcColorFirstColorlessShape(Color.green, targetedAvatarIndex);

                } else if (voteResult == EVoteResult.No)
                {
                    GameManager.instance.ReceiveVoteUI.RpcColorFirstColorlessShape(Color.red, targetedAvatarIndex);
                }
                
            }
        }

        public override int GetAvatarIndex()
        {
            return playerIndex;
        }
    }
}