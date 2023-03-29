using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class DownloadBar : MonoBehaviour
    {
        
        public Slider slider;
        
        public void SetDownloadBar(int taskNumber)
        {
            slider.maxValue = taskNumber;
            slider.value = taskNumber;
        }

        public void UpdateDownloadbar(int taskDone)
        {
            slider.value = taskDone;
        }
    }
}

