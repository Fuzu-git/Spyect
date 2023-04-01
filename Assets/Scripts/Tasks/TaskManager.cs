using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField]
        private int totalTask;
        [SerializeField]
        private int actualTask = 0; 
        
        [SerializeField]
        private  Slider taskBar;
        
        public void Awake()
        {
            totalTask = GameObject.FindGameObjectsWithTag("Task").Length;
            taskBar.maxValue = totalTask; 
        }

        public void AddTask()
        {
            actualTask++; 
            taskBar.value = actualTask;
        }
    }
}
