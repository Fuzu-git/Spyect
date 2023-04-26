using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lobby;
using Member;
using Member.Player;
using Member.Player.DataPlayer;
using Mirror;
using TMPro;
using UI.Ranking;
using UI.VoteUI;
using UnityEngine;

public struct StartGameMessage : NetworkMessage
{
}

public struct PlayerVote
{
    public PlayerBehaviour origin; // player who voted; 
    public AvatarBehaviour target; // player who's been voted; 
    public EVoteResult voteResult; //vote decision
}

public enum EVoteResult
{
    Yes,
    No,
    White
}

public class GameManager : MonoBehaviour
{
    public Action onGameStarted;

    public List<GameObject> playerList = new List<GameObject>();
    public List<PlayerBehaviour> deadPlayerList = new List<PlayerBehaviour>();

    public List<GameObject> aiList = new List<GameObject>();
    public List<GameObject> memberList = new List<GameObject>();

    public static GameManager instance;

    public bool totalPlayerUpdateOccured = false;

    private List<PlayerVote> _votedPlayer = new List<PlayerVote>(); //votes

    public ProfileFiller ProfileFiller;
    public SendVoteUI SendVoteUI;
    public ReceiveVoteUI ReceiveVoteUI;

    public GameObject shapeVoteImage;

    public GameObject victoryPanel;
    public TMP_Text victoryMessage;

    private PlayerNumberCounter _playerNumberCounter;
    private List<NetworkGamePlayer> _lobbyGamePlayerList = new List<NetworkGamePlayer>();

    [SerializeField] private RankingUIManager rankingUIManager; 

    public void Awake()
    {
        instance = this;
        GameObject temp = GameObject.FindGameObjectWithTag("PlayerCounter");

        _playerNumberCounter = temp.GetComponent<PlayerNumberCounter>();
    }

    IEnumerator Start()
    {
        NetworkClient.RegisterHandler<StartGameMessage>(OnStartGameReceived);

        yield return new WaitUntil(() => NetworkServer.active || NetworkClient.active);

        NetworkServer.SendToReady(new StartGameMessage());

        if (NetworkClient.active)
        {
            onGameStarted?.Invoke();
        }

        GameObject[] gamePlayerPrefabs = GameObject.FindGameObjectsWithTag("GamePlayerPrefab");
        foreach (GameObject gamePlayer in gamePlayerPrefabs)
        {
            _lobbyGamePlayerList.Add(gamePlayer.GetComponent<NetworkGamePlayer>());
        }

        _lobbyGamePlayerList = _lobbyGamePlayerList.OrderBy(x => x.GetPlayerIndex()).ToList();
    }

    public void AddPlayer(GameObject player)
    {
        playerList.Add(player);
    }

    public void AddAI(GameObject ai)
    {
        aiList.Add(ai);
    }

    public void AddMember(GameObject member)
    {
        memberList.Add(member);
    }

    private void OnStartGameReceived(StartGameMessage msg)
    {
        onGameStarted?.Invoke();
    }

    internal bool Vote(PlayerBehaviour origin, AvatarBehaviour target, EVoteResult voteResult)
    {
        int index = _votedPlayer.FindIndex(x => x.origin == origin && x.target == target);
        if (index == -1)
        {
            _votedPlayer.Add(new PlayerVote {origin = origin, target = target, voteResult = voteResult});

            return true;
        }

        return false;
    }

    public void CheckAllVotes(int targetedAvatarIndex)
    {
        List<PlayerVote> voteAgainstTarget = _votedPlayer.Where
        (x => x.target.GetAvatarIndex()
              == targetedAvatarIndex).ToList();


        if (playerList.Count == voteAgainstTarget.Count)
        {
            Dictionary<AvatarBehaviour, int> votes = new Dictionary<AvatarBehaviour, int>();
            Dictionary<AvatarBehaviour, int> voteNumber = new Dictionary<AvatarBehaviour, int>();

            foreach (PlayerVote vote in _votedPlayer)
            {
                if (vote.voteResult == EVoteResult.Yes)
                {
                    if (voteNumber.ContainsKey(vote.target))
                    {
                        ++voteNumber[vote.target];
                    }
                    else
                    {
                        voteNumber.Add(vote.target, 1);
                    }

                    if (votes.ContainsKey(vote.target))
                    {
                        ++votes[vote.target];
                    }
                    else
                    {
                        votes.Add(vote.target, 1);
                    }
                }
                else if (vote.voteResult == EVoteResult.No)
                {
                    if (voteNumber.ContainsKey(vote.target))
                    {
                        ++voteNumber[vote.target];
                    }
                    else
                    {
                        voteNumber.Add(vote.target, 1);
                    }

                    if (votes.ContainsKey(vote.target))
                    {
                        --votes[vote.target];
                    }
                    else
                    {
                        votes.Add(vote.target, -1);
                    }
                }

                if (voteNumber[vote.target] == playerList.Count)
                {
                    _votedPlayer = _votedPlayer.Except(voteAgainstTarget).ToList();
                    ReceiveVoteUI.RpcCloseVoteContent(vote.target.GetAvatarIndex());
                }
            }

            foreach (var vote in votes)
            {
                if (vote.Value > 0)
                {
                    vote.Key.State = PlayerState.Dead;
                }
            }
        }
    }

    public void CheckVictory()
    {
        // Assign it on suspection kill when DONE 

        int deadPlayers = 0;
        PlayerBehaviour lastPlayer = null;
        PlayerBehaviour thisTurnDead = null; 
            
        foreach (var player in playerList)
        {
            PlayerBehaviour currentPlayer = player.GetComponent<PlayerBehaviour>();

            if (currentPlayer.State == PlayerState.Dead)
            {
                if (!deadPlayerList.Contains(currentPlayer))
                {
                    deadPlayerList.Add(currentPlayer); 
                    thisTurnDead = currentPlayer; 
                }
                deadPlayers++;
            }
            else
            {
                lastPlayer = currentPlayer;
            }
        }
        
        
        if (thisTurnDead != null)
        {
            thisTurnDead.playerRank = playerList.Count - (deadPlayers -1);
        }
        
        if (deadPlayers == playerList.Count - 1)
        {
            lastPlayer.playerRank = playerList.Count - deadPlayers; 
            victoryPanel.SetActive(true);
            victoryMessage.text =
                $" {_lobbyGamePlayerList[lastPlayer.playerIndex].GetDisplayName()} ({lastPlayer.playerNameText.text}) a gagné !";
        }
    }

    public void UpdateRankingUIManager()
    {
        if (rankingUIManager.gameObject.activeInHierarchy)
        {
            rankingUIManager.FillRankingMessage();
        }
    }

    public bool IsAlreadySuspected(int playerIndex)
    {
        foreach (var vote in _votedPlayer)
        {
            if (playerIndex == vote.target.profileIndex)
            {
                return true;
            }
        }
        return false;
    }

    public NetworkGamePlayer GetNetworkGamePlayer(int playerIndex)
    {
        return _lobbyGamePlayerList.Find(x => x.GetPlayerIndex() == playerIndex);
    }
}