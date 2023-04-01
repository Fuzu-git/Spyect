using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Tasks
{
    public class DownloadBar : GenericTask
    {
        public Slider slider;
        public float duration; 

        public override void PlayMiniGame()
        {
            if (_taskIsDone)
            {
                return; 
            }
            base.PlayMiniGame();
            slider.value = 0;
            slider.DOValue(1, duration).OnComplete(CloseMiniGame);
        }
    }
}

