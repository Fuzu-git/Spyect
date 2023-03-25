using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Member;
using Member.Player.DataPlayer;
using Mirror;
using UI.VoteUI;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    //[SerializeField] private List<GameObject> showPlayerList = playerList;
    public List<GameObject> aiList = new List<GameObject>();
    public List<GameObject> memberList = new List<GameObject>();

    public static GameManager instance;

    public bool totalPlayerUpdateOccured = false;

    private List<PlayerVote> _votedPlayer = new List<PlayerVote>(); //votes

    public ProfileFiller ProfileFiller;
    public SendVoteUI SendVoteUI;
    public ReceiveVoteUI ReceiveVoteUI;


    public GameObject shapeVoteImage;


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
        Debug.Log("JOJO " + (origin == null) + " " + (target.name + " " + target.GetAvatarIndex()));

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
        List<PlayerVote> voteAgainstTarget = _votedPlayer.Where(x => x.target.GetAvatarIndex() == targetedAvatarIndex).ToList();
        
        
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
                    //PlayerStateIsDead(vote.Key.gameObject;)
                    Debug.Log(ReceiveVoteUI.receiveVoteContentList.Count);
                }
            }
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
}