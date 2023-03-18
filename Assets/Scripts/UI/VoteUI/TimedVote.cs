using UnityEngine;
using UnityEngine.UI;
namespace UI.VoteUI
{
    public class TimedVote : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;
        public Image fill; 
        
        public void SetVoteTimeBar(int voteTime)
        {
            slider.maxValue = voteTime;
            slider.value = voteTime;

            fill.color = gradient.Evaluate(1f);
        }

        public void Update()
        {
            fill.color = gradient.Evaluate(slider.normalizedValue);
            while (slider.value > 0)
            {
                slider.value -= Time.deltaTime; 
            }
        }
    }
}
