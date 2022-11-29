using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.DataPlayer
{
    [CreateAssetMenu]
    public class CharInfoScriptable : UnityEngine.ScriptableObject
    {
        public TMP_Text playerInGameName;
        public Button suspectedButton;
        public Image playerInGameImage;    
    }
}
