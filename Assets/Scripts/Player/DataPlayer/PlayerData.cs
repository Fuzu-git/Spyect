using UnityEngine;
using UnityEngine.UI;

namespace Player.DataPlayer
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public string playerInGameName;
        public Sprite playerInGameImage;    
    }
}