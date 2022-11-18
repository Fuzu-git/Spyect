using System.Collections;
using TMPro;
using UnityEngine;

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
        
            int i = 0;

        //Debug.Log(GameManager.teamList);
        //Debug.Log(GameManager.teamList.Count);
            foreach (var element in GameManager.teamList)
            {
                if (player.gameObject == element.player1 || player.gameObject == element.player2)
                {
                    roleText.text = "Vous êtes à la solde de " + GameManager.teamList[i].teamName + "!";
                }
                i++; 
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