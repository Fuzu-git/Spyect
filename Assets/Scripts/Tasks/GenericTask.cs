using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tasks
{
    public class GenericTask : MonoBehaviour
    {
        private GameObject _highlight;

        public TaskManager taskManager;

        public TaskData taskData;
        public Interactable interactable; 
        
        public  virtual void PlayMiniGame()
        {
            gameObject.SetActive(true);
        }

        protected void CloseMiniGame()
        {
            gameObject.SetActive(false);
            
            taskData.taskIsDone = true; 
            interactable.CleanButtonState();
            
            Debug.Log(gameObject.activeInHierarchy);
            taskManager.AddTask();
        }
    }
}
