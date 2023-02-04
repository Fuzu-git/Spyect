using Lobby;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumberCounter : NetworkBehaviour
{
    [SyncVar]
    public int PlayerNumber = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void CountPlayer(int i)
    {
        PlayerNumber = i;
    }
}
