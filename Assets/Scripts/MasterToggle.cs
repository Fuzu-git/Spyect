using System.Collections;
using System.Collections.Generic;
using Member;
using UnityEngine;
using Member.Player.DataPlayer;
using UI.VoteUI;
using UnityEngine.UI;

public class MasterToggle : MonoBehaviour
{        
    public Button useButton;
    [SerializeField]
    private OpenSendVoteUI openSendVoteUI; 
    public void OnTriggerEnter(Collider other)
    {
        if (PlayerBehaviour.local != null && other.gameObject == PlayerBehaviour.local.gameObject && PlayerBehaviour.local.State == PlayerState.Alive)
        {
            useButton.interactable = true;
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(openSendVoteUI.OpenUI);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (PlayerBehaviour.local != null && other.gameObject == PlayerBehaviour.local.gameObject)
        {
            CleanButtonState();
        }
    }

    public void CleanButtonState()
    {
        useButton.interactable = false;  
        useButton.onClick.RemoveAllListeners();
    }
}
