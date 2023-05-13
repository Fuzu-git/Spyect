using System.Collections;
using Member;
using Member.Player.DataPlayer;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BeginUI : MonoBehaviour
    {
        public CanvasGroup group;
        public TMP_Text roleText;
        public GameManager gameManager;

        void Start()
        {
            gameManager.onGameStarted += OnGameStart;
        }

        private void OnDestroy()
        {
            gameManager.onGameStarted -= OnGameStart;
        }

        private void OnGameStart()
        {
            StartCoroutine(WaitingFade());
        }


        private IEnumerator WaitingFade()
        {
            yield return new WaitUntil(() => PlayerBehaviour.local != null);
            yield return new WaitUntil(() => !string.IsNullOrEmpty(PlayerBehaviour.local.playerInGameName));

            yield return null; 

            roleText.text = "There is your new identity... " + PlayerBehaviour.local.playerInGameName + ".";
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