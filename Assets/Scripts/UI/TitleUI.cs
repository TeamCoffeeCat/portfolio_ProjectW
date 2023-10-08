using ProjectW.DB;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW
{
    public class TitleUI : MonoBehaviour
    {
        public Button StartButton;
        public Button ContinueButton;

        public GameObject NicknameInput;
        public TMP_InputField NicknameInputField;
        public Button NicknameConfirmButton;
        public Button InputFieldCloseButton;

        public GameObject ErrorMessage;
        public Button ErrorMessageButton;

        private void Start()
        {
            StartButton.onClick.AddListener(OnClickedStartButton);
            NicknameConfirmButton.onClick.AddListener(OnClickedNicknameConfirmButton);
            InputFieldCloseButton.onClick.AddListener(OnClickedInputFieldCloseButton);
            ErrorMessageButton.onClick.AddListener(OnClickedErrorMessageButton);
            ContinueButton.onClick.AddListener(OnClickedContinueButton);
        }

        private void OnClickedStartButton()
        {
            StartButton.gameObject.SetActive(false);
            ContinueButton.gameObject.SetActive(false);

            NicknameInput.SetActive(true);
        }

        private void OnClickedNicknameConfirmButton()
        {
            var length = NicknameInputField.text.Length;

            if (length < 3 || length > 10)
            {
                ErrorMessage.SetActive(true);
                return;
            }
            DataManager.CreateNewCharacter(NicknameInputField.text);
        }

        private void OnClickedInputFieldCloseButton()
        {
            NicknameInput.SetActive(false);

            StartButton.gameObject.SetActive(true);
            ContinueButton.gameObject.SetActive(true);
        }

        private void OnClickedErrorMessageButton()
        {
            ErrorMessage.SetActive(false);
        }

        private void OnClickedContinueButton()
        {
            UIPresenter.Instance.SetActiveSaveDataUI(true);
        }
    }
}