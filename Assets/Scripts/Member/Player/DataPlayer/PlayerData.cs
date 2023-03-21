using UnityEngine;

namespace Member.Player.DataPlayer
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public string playerInGameName;
        public Sprite playerInGameImage;
        public RuntimeAnimatorController avatarAnimator; 
    }
}