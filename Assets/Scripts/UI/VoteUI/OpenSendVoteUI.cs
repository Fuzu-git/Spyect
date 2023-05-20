using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class OpenSendVoteUI : MonoBehaviour
    {
        public SendVoteUI sendVoteUI;
        public void OpenUI()
        {
            sendVoteUI.UpdateGrid();
        }
    }
}