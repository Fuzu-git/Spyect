using System.Collections;
using Mirror;
using UnityEngine;

namespace Player.DataPlayer
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public static PlayerBehaviour local;
        
        //[SyncVar] not required -> GameManager.instance called. 
        public bool canMove = true;

        private IEnumerator Start()
        {
            if (isLocalPlayer)
            {
                local = this; 
            
                GameManager.playerList.Add(gameObject);
            }

            if (GameManager.instance)
            {
                //GameManager.instance.AddPlayer(this.gameObject);
                canMove = false;
                yield return new WaitForSeconds(5);
                canMove = true; 
            }
        }
    }
}
