using System;
using System.Collections;
using Member;
using Member.Player.DataPlayer;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;
using UnityEditor.TextCore.Text;
using UnityEngine.Serialization;

namespace Tasks
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private GameObject miniGame;

        public Button useButton;

        [SerializeField]
        private GenericTask genericTask;

        private GameObject _highlight;

        private IEnumerator Start()
        {
            while (!genericTask._taskIsDone)
            {
                _highlight.SetActive(true);
                yield return new WaitForSeconds(1f);
                _highlight.SetActive(false);
                yield return new WaitForSeconds(1f);
            } 
            
            if (genericTask._taskIsDone)
            {
                _highlight.SetActive(false); 
            }
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerBehaviour.local.gameObject)
            {
                useButton.interactable = true;  
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(genericTask.PlayMiniGame);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject == PlayerBehaviour.local.gameObject)
            {
                useButton.interactable = false;  
                useButton.onClick.RemoveAllListeners();
            }
        }

        private void OnEnable()
        {
            _highlight = transform.GetChild(0).gameObject;
        }
    }
}