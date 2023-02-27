using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : AvatarBehaviour
{
    protected override void OnPlayerStateChanged(PlayerState oldState, PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Dead:
                Destroy(gameObject);
                //joueur désigné mort. (lien vers UI)
                // if VoteYes, playerVote are suspected.
                break;
        }
    }
}
