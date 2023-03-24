using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class VoteColorShape : MonoBehaviour
    {
        public static GameObject shapeVoteImage;

        public static void ColorRed()
        {
            //shapeVoteImage.GetComponent<Image>().color = Color.red;
            GameManager.instance.shapeVoteImage.GetComponent<Image>().color = Color.red;
        }

        public static void ColorGreen()
        {
            //shapeVoteImage.GetComponent<Image>().color = Color.green;
            GameManager.instance.shapeVoteImage.GetComponent<Image>().color = Color.green;
            
        }
    }
}
