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
        
        public bool _taskIsDone = false;
        
        public  virtual void PlayMiniGame()
        {
            gameObject.SetActive(true);
        }

        protected void CloseMiniGame()
        {
            gameObject.SetActive(false);
            Debug.Log(gameObject.activeInHierarchy);
            _taskIsDone = true; 
            taskManager.AddTask();
        }
        
        
    }
}
