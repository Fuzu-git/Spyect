using System.Collections;
using System.Collections.Generic;
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
        protected internal PlayerState state = PlayerState.Alive;

        public float movementSpeed = 5f;
        public static bool canMove = true;
        public int profileIndex { get; protected set; } = -1;
        public TMP_Text playerNameText;
        protected string playerInGameName;
        protected ProfileFiller profileFillerComponent;
        protected GameManager gameManager;

        protected abstract void OnPlayerStateChanged(PlayerState oldState, PlayerState newState);

        protected virtual IEnumerator Start()
        {
            GameObject go = GameObject.FindGameObjectWithTag("GameManager");
            gameManager = go.GetComponent<GameManager>();
            profileFillerComponent = gameManager.ProfileFiller;

            CmdSelectRandomProfile();

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
            }
        }

        //[ClientRpc]
        protected void RpcSendProfilToClient(int profileIndex)
        {
            StartCoroutine(SendProfilToClientCo(profileIndex));
        }

        protected IEnumerator SendProfilToClientCo(int profileIndex)
        {
            while (profileFillerComponent == null)
            {
                yield return null;
            }
            PlayerData currentSo = profileFillerComponent.profiles[profileIndex];
            playerInGameName = currentSo.playerInGameName;
            playerNameText.text = playerInGameName;
        }

        //[Command]
        protected virtual void CmdSelectRandomProfile()
        {
            
        }
    }
}