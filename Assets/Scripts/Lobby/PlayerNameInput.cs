using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class PlayerNameInput : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button continueButton;
    
        public static string DisplayName { get; private set; }

        private const string PlayerPrefsNameKey = "PlayerName";

        private void Start()
        {
            SetupInputField();
            nameInputField.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

        }

        private void ValueChangeCheck()
        {
            SetPlayerName(name);
        }

        private void SetupInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
            {
                return; 
            }

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
            nameInputField.text = defaultName;
            SetPlayerName(defaultName);
        }

        public void SetPlayerName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                continueButton.interactable = false; 
            }
            else
            {
                continueButton.interactable = true; 
            }
            //continueButton.interactable = !string.IsNullOrWhiteSpace(name);
            
        }

        public void SavePlayerName()
        {
            DisplayName = nameInputField.text; 
            PlayerPrefs.SetString(PlayerPrefsNameKey,DisplayName);
        }
    }
}
