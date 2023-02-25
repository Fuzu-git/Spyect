using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using  Player.DataPlayer;
using TMPro;

public enum PlayerState
{
    Alive, 
    Spectate
}
public abstract class AvatarBehaviour : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPlayerStateChanged))] 
    protected internal PlayerState state = PlayerState.Alive; 
        
    public static bool canMove = true;
    
    public GameObject instantiatedProfileFiller;
    public int profileIndex { get; protected set; } = -1 ;
    public TMP_Text playerNameText;
    protected string playerInGameName;
    protected ProfileFiller profileFillerComponent;

    protected abstract void OnPlayerStateChanged(PlayerState oldState, PlayerState newState);
    
    protected virtual IEnumerator Start() 
    {
        if (isServer)
        {
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

    [ClientRpc]
    private void SendProfilToClient(int profileIndex)
    {
        if (profileFillerComponent == null)
        {
            instantiatedProfileFiller = GameObject.FindGameObjectWithTag("profileFiller");
            profileFillerComponent = instantiatedProfileFiller.GetComponent<ProfileFiller>();
        }
        PlayerData currentSo = profileFillerComponent.profiles[profileIndex];
        playerInGameName = currentSo.playerInGameName;
        playerNameText.text = playerInGameName;
    }
    
    protected IEnumerator SelectRandomProfile()
    {
        while (instantiatedProfileFiller == null)
        {
            instantiatedProfileFiller = GameObject.FindGameObjectWithTag("profileFiller");
            yield return new WaitForSeconds(0.1f); 
        }

        ProfileFiller profileFillerComponent = instantiatedProfileFiller.GetComponent<ProfileFiller>();

           
        yield return new WaitForSeconds(0.1f);
            
        profileIndex = profileFillerComponent.GetIndex(connectionToClient.connectionId);


        SendProfilToClient(profileIndex);
    }
}
