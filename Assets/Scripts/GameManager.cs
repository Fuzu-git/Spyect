using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player.DataPlayer;
using UnityEngine;
using Random = UnityEngine.Random;

public struct  StartGameMessage : NetworkMessage{}

public class GameManager : MonoBehaviour
{
    public static Action onGameStarted;
     
     public static readonly List<GameObject> playerList = new List<GameObject>();
     
     [SerializeField] private List<GameObject> players = new List<GameObject>(); //gotta check what use is this...

     public static GameManager instance;

    public void Awake()
    {
        instance = this; 
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

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
    }
    

    private void OnStartGameReceived(StartGameMessage msg)
    {
        onGameStarted?.Invoke();
    }
}