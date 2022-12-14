using Mirror;
using UnityEngine;

namespace Lobby
{
    public static class NetworkHelper
    {
        public static GameObject GetGameObjectFromNetIdValue(uint netIdValue, bool isServer)
        {
            NetworkIdentity identity = null;
            if (isServer)
            {
                NetworkConnectionToClient toClient;
                NetworkServer.connections.TryGetValue((int)netIdValue, out toClient);
                if(toClient != null)
                {
                    identity = toClient.identity;
                }
            }
            else
            {
                NetworkClient.spawned.TryGetValue(netIdValue, out identity);
            }

            if (identity)
            {
                return identity.gameObject;
            }

            return null;
        }
    }
}