using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Player.DataPlayer;
using UI.VoteUI;
using UnityEngine;
using UnityEngine.PlayerLoop;

public struct StartGameMessage : NetworkMessage
{
}

public struct PlayerVote
{
    public PlayerBehaviour origin; // player who voted; 
    public PlayerBehaviour target; // player who's been voted; 
}

public class GameManager : MonoBehaviour
{
    public static Action onGameStarted;

    public static readonly List<GameObject> playerList = new List<GameObject>();
    [SerializeField] private List<GameObject> showPlayerList = playerList;
    public static GameManager instance;

    public bool totalPlayerUpdateOccured = false;

    private List<PlayerVote> _votedPlayer = new List<PlayerVote>();

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
        playerList.Add(player);
    }

    private void OnStartGameReceived(StartGameMessage msg)
    {
        onGameStarted?.Invoke();
    }

    internal bool Vote(PlayerBehaviour origin, PlayerBehaviour target)
    {
        int index = _votedPlayer.FindIndex(x => x.origin == origin);
        if (index == -1)
        {
            _votedPlayer.Add(new PlayerVote {origin = origin, target = target});
            return true;
        }

        return false;
    }

    public void CheckAllVotes()
    {
        if (playerList.Count == _votedPlayer.Count)
        {
            Dictionary<PlayerBehaviour, int> votes = new Dictionary<PlayerBehaviour, int>();

            foreach (PlayerVote vote in _votedPlayer)
            {
                if (votes.ContainsKey(vote.target))
                {
                    ++votes[vote.target];
                }
                else
                {
                    votes.Add(vote.target, 1);
                }
            }

            List<KeyValuePair<PlayerBehaviour, int>> order = votes.OrderByDescending(x => x.Value).ToList();
            if (order.Count() <= 1 || order[0].Value != order[1].Value)
            {
                PlayerBehaviour player = order[0].Key;
                player.state = PlayerState.Spectate;
            }
        }
    }
}