using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public struct  StartGameMessage : NetworkMessage{}

public struct TeamStruct
{
    public string teamName;
    
    public GameObject player1;
    public GameObject player2;
};

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public void Awake()
    {
        instance = this; 
    }
    
    public static Action onGameStarted;
    
    public static readonly List<TeamStruct> teamList = new List<TeamStruct>();
    public static readonly List<GameObject> playerList = new List<GameObject>();
    [SerializeField] private List<GameObject> players = new List<GameObject>();


    IEnumerator Start()
    {
        NetworkClient.RegisterHandler<StartGameMessage>(OnStartGameReceived);

        yield return new WaitUntil(() => NetworkServer.active);
        yield return new WaitUntil(() => NetworkClient.active);
        
        
#region MakingRandomTeams

        List<GameObject> tempList = playerList;
        
                int numberPlayer = tempList.Count;
                    
                for (int i = 0; i < numberPlayer; i+=2)
                {
                    int randP1 = Random.Range(0, tempList.Count);
                    int randP2 = Random.Range(0, tempList.Count);
                    while (randP1 == randP2)
                    {
                        randP2 = Random.Range(0, tempList.Count);
                    }
        
                    TeamStruct agence = new TeamStruct();
                    
                    agence.player1 = tempList[randP1];
                    agence.player2 = tempList[randP2];
                    agence.teamName = "Agence " + i / 2;
        
                    
                    teamList.Add(agence);
                    
                    if (randP1 > randP2)
                    {
                        tempList.RemoveAt(randP1);
                        tempList.RemoveAt(randP2);
                    }
                    else
                    {
                        tempList.RemoveAt(randP2);
                        tempList.RemoveAt(randP1);
                    }
                }

                #endregion
        

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