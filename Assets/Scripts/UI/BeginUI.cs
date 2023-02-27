using System.Collections;
using Member.Player.DataPlayer;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BeginUI : MonoBehaviour
    {
        public CanvasGroup group;
        public TMP_Text roleText;

        void Start()
        {
            GameManager.onGameStarted += OnGameStart;
        }

        private void OnDestroy()
        {
            GameManager.onGameStarted -= OnGameStart; 
        }

        private void OnGameStart()
        {
            PlayerBehaviour player = PlayerBehaviour.local;
            
            foreach (GameObject element in GameManager.playerList)
            {
                    roleText.text = "There is your new identity... " + player.name + ".";
            }
            StartCoroutine(DoFade());
        }

        private IEnumerator DoFade()
        {
            yield return new WaitForSeconds(2);

            float time = 0;
            float duration = 2;
 
            while (time < duration)
            {
                group.alpha = Mathf.Lerp(1, 0, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            group.alpha = 0;
            group.blocksRaycasts = false;
            group.interactable = false; 
        }
    }
}