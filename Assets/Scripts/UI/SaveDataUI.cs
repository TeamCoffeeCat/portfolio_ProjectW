using ProjectW.DB;
using ProjectW.Dummy;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW
{
    public class SaveDataUI : MonoBehaviour
    {
        public Transform SlotList;
        public Button CloseButton;
        private SaveSlot[] SaveSlots;

        public void Initialize()
        {
            CloseButton.onClick.AddListener(() => gameObject.SetActive(false));

            SaveSlots = new SaveSlot[SlotList.childCount];
            for (int i = 0; i < SlotList.childCount; i++)
            {
                SaveSlots[i] = SlotList.GetChild(i).GetComponent<SaveSlot>();
            }

            for (int i = 0; i < SaveSlots.Length; i++)
            {
                if (DataManager.Instance.IsExistSaveData(i))
                {
                    var userData = DataManager.Instance.LoadUserData(i);

                    RefreshSlot(SaveSlots[i].TextMesh, userData);
                    SaveSlots[i].SlotButton.onClick.AddListener(() =>
                    {
                        DataManager.Instance.LoadGame(userData);
                        gameObject.SetActive(false);
                    });
                }
            }
        }

        public void SetSaveMode()
        {
            for (int i = 0; i < SaveSlots.Length; i++)
            {
                int index = i;
                SaveSlots[i].SlotButton.onClick.RemoveAllListeners();
                SaveSlots[i].SlotButton.onClick.AddListener(() =>
                {
                    DataManager.Instance.SaveUserData(index);
                    RefreshSlot(SaveSlots[index].TextMesh, DummyServer.Instance.userData);
                });
            }
        }

        private void RefreshSlot(TextMeshProUGUI textMesh, UserData userData)
        {
            textMesh.text = $"이름 : {userData.dtoAccount.nickname}\n" +
                $"레벨 : {userData.dtoCharacter.level}";
        }
    }
}