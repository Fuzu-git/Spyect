using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Member.Player.DataPlayer;
using Mirror;
using TMPro;
using UnityEngine;

namespace Member
{
    public enum PlayerState
    {
        Alive,
        Dead,
        Spectate
    }

    public abstract class AvatarBehaviour : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnPlayerStateChanged))]
        protected internal PlayerState State = PlayerState.Alive;

        public float movementSpeed = 5f;
        public bool canMove = false;
        public int profileIndex { get; protected set; } = -1;
        protected ProfileFiller ProfileFillerComponent;
        
        public TMP_Text playerNameText;
        public string playerInGameName = null; 
        public SpriteRenderer spriteRenderer;
        
        public Animator animator;
        
        protected GameManager GameManager;
        
        public abstract void OnPlayerStateChanged(PlayerState oldState, PlayerState newState);

        protected virtual IEnumerator Start()
        {
            yield return StartCoroutine(GetReferences());
            StartCoroutine(AllowMovement());
        }
        
        protected IEnumerator GetReferences()
        {
            GameObject go = GameObject.FindGameObjectWithTag("GameManager");
            GameManager = go.GetComponent<GameManager>();
            ProfileFillerComponent = GameManager.ProfileFiller;
            while (!ProfileFillerComponent.IsReady)
            {
                yield return null;
            }
        }

        protected IEnumerator AllowMovement()
        {
            if (GameManager.instance)
            {
                if (gameObject.CompareTag("Player"))
                {
                    GameManager.instance.AddPlayer(this.gameObject);
                }
                else if (gameObject.CompareTag("AI"))
                {
                    GameManager.instance.AddAI(this.gameObject);
                }
                GameManager.instance.AddMember(gameObject);
                canMove = false;
                yield return new WaitForSeconds(5);
                canMove = true;
                if (isLocalPlayer)
                {
                    GameManager.memberList = GameManager.memberList.OrderBy(x => x.GetComponent<AvatarBehaviour>().GetAvatarIndex()).ToList();
                }
            }
        }

        protected virtual IEnumerator WaitForProfiller()
        {
            while (ProfileFillerComponent == null)
            {
                yield return null;
            }
            SelectRandomProfile();
        }

        protected void Flip(float velocity)
        {
            if (velocity > 0f && spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
                CmdFlipX(false);
            }
            else if (velocity < 0f && !spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true; 
                CmdFlipX(true);
            }
        }

        [Command]
        private void CmdFlipX(bool isSpriteRendererFlipped)
        {
            spriteRenderer.flipX = isSpriteRendererFlipped;
            RpcFlipX(isSpriteRendererFlipped);
        }
        
        [ClientRpc]
        private void RpcFlipX(bool isSpriteRendererFlipped)
        {
            if (!isLocalPlayer)
            {
                spriteRenderer.flipX = isSpriteRendererFlipped; 
            }
        }
        protected void SendProfilToClient(int profileIndex)
        {
            PlayerData currentSo = ProfileFillerComponent.profiles[profileIndex];
            playerInGameName = currentSo.playerInGameName;
            playerNameText.text = playerInGameName;
            animator.runtimeAnimatorController = currentSo.avatarAnimator;
        }

        public virtual int GetAvatarIndex()
        {
            return -1;
        }

        protected virtual void SelectRandomProfile()
        {
        }
    }
}