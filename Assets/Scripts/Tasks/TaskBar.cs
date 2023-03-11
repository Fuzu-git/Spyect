using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class TaskBar : MonoBehaviour
    {
        public Slider slider;
        
        public void SetTaskBar(int taskNumber)
        {
            slider.maxValue = taskNumber;
            slider.value = taskNumber;
        }

        public void UpdateTaskbar(int taskDone)
        {
            slider.value = taskDone;
        }
    }
}
