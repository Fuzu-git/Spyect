using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Player.DataPlayer;
using TMPro;
using Random = UnityEngine.Random;
using Lobby;

public struct  StartGameMessage : NetworkMessage{}

public class GameManager : NetworkBehaviour
{
    public static Action onGameStarted;
     
     public static readonly List<GameObject> playerList = new List<GameObject>();

     public readonly SyncList<int> playerIndex = new SyncList<int>();
     
     //[SerializeField] private List<GameObject> players = new List<GameObject>(); //gotta check what use is this... (none)

     public static GameManager instance;

     //[ClientRpc]
     public List<PlayerData> profilSO;
     
     public TMP_Text playerInGameName;

     
    public void Awake()
    {
        for (int i = 0; i <= playerList.Count; i++)
        {
            playerIndex.Add(i);
        }

        instance = this;
        SelectRandomProfile();

        
    }

    IEnumerator Start()
    {
        NetworkClient.RegisterHandler<StartGameMessage>(OnStartGameReceived);

        yield return new WaitUntil(() => NetworkServer.active);
        yield return new WaitUntil(() => NetworkClient.active);

        NetworkServer.SendToReady(new StartGameMessage());
        
        if (NetworkClient.active)
        {
            onGameStarted?.Invoke();
        }
    }

    /*public void AddPlayer(GameObject player) //goes with List<GameObject> players
    {
        players.Add(player);
    }*/
    
    [Server]
    void SelectRandomProfile()
    {
        // List<int> tempList = new List<int>();
        //
        // for (int i = 0; i < playerList.Count; i++)
        // {
        //     tempList.Add(i);
        // }

        int playerIndexInt;
        foreach (GameObject player in playerList)
        {
            playerIndexInt = playerIndex[Random.Range(0, playerIndex.Count)];
            
            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();
            GetPlayerProfile(playerIdentity.connectionToClient, playerIndexInt, playerIdentity.netId);
            
            playerIndex.RemoveAt(playerIndexInt);
        }
    }

    [TargetRpc]
    void GetPlayerProfile(NetworkConnection player, int profileIndex, uint playerID)
    {
        GameObject myAvater = NetworkHelper.GetGameObjectFromNetIdValue(playerID, false);
        
        PlayerData currentSO = profilSO[profileIndex];
        string currentSO_Name = currentSO.playerInGameName;
        //Sprite currentSO_Image = currentSO.playerInGameImage;
        //playerInGameImage = currentSO_Image;

        myAvater.GetComponent<PlayerBehaviour>().playerName_Text.text = currentSO_Name;
    }   

    private void OnStartGameReceived(StartGameMessage msg)
    {
        onGameStarted?.Invoke();
    }
}