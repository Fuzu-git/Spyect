using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VoteUI
{
    public class GridVoteContent : MonoBehaviour
    {
        public TMP_Text _playerInGameName;
        public Image _playerInGameAvatar;

        public Button _playerSuspected;

        public SendVoteUI sendVoteUI;

        private void Start()
        {
            _playerSuspected.onClick.AddListener(SuspectedButtonClicked);
        }

        private void SuspectedButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}