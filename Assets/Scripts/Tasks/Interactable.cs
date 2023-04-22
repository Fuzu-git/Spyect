using System;
using System.Collections;
using Member;
using Member.Player.DataPlayer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Tasks
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] public GameObject miniGame;

        public Button useButton;

        [SerializeField]
        public GenericTask genericTask;

        private GameObject _highlight;

        [SerializeField] 
        public TaskData task; 
        
        private IEnumerator Start()
        {
            while (!task.taskIsDone)
            {
                _highlight.SetActive(true);
                yield return new WaitForSeconds(1f);
                _highlight.SetActive(false);
                yield return new WaitForSeconds(1f);
            } 
            
            if (task.taskIsDone)
            {
                _highlight.SetActive(false); 
            }
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (PlayerBehaviour.local != null && other.gameObject == PlayerBehaviour.local.gameObject && !task.taskIsDone)
            {
                genericTask.interactable = this; 
                genericTask.taskData = task;
                useButton.interactable = true;
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(genericTask.PlayMiniGame);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (PlayerBehaviour.local != null && other.gameObject == PlayerBehaviour.local.gameObject)
            {
                CleanButtonState();
            }
        }

        public void CleanButtonState()
        {
            genericTask.taskData = null; 
            useButton.interactable = false;  
            useButton.onClick.RemoveAllListeners();
        }
        
        private void OnEnable()
        {
            _highlight = transform.GetChild(0).gameObject;
        }
    }
}