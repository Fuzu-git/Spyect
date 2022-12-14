using System.Collections;
using Mirror;
using UnityEngine;
using TMPro;

namespace Player.DataPlayer
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public static PlayerBehaviour local;
        
        //[SyncVar] not required -> GameManager.instance called. 
        public bool canMove = true;

        
        public TMP_Text playerName_Text;
        
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
