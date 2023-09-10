using ProjectW.DB;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectW
{
    public class ItemSlot : MonoBehaviour
    {
        public BoItem boItem;
        public Image Icon;
        public GameObject ItemCount;
        public TextMeshProUGUI CountText;
        public Button UseButton;

        private bool isExist;
        public bool IsExist
        {
            get { return isExist; }
            set
            {
                isExist = value;

                if (!isExist)
                {
                    DeActiveAll();
                }
            }
        }

        private void Start()
        {
            AddUseButtonEvent();
        }

        public void SetSlot(BoItem boItem)
        {
            if (boItem == null)
                return;

            IsExist = true;
            this.boItem = boItem;
            Icon.sprite = Resources.Load<Sprite>(boItem.SdItem.spritePath); // 아이템 아이콘
            Icon.color = SetHexColor(boItem.SdItem.colorCode); // 아이콘 컬러
            CountText.text = boItem.ItemCount.ToString(); // 아이템 갯수

            // 아이템 슬롯 활성화
            Icon.gameObject.SetActive(true);
            ItemCount.SetActive(true);
        }

        public void RefreshSlot(BoItem boItem)
        {
            if (IsExist)
            {
                CountText.text = boItem.ItemCount.ToString();
                return;
            }

            SetSlot(boItem);
        }

        private void AddUseButtonEvent()
        {
            UseButton.onClick.AddListener(() => UsedItem());

            EventTrigger.Entry enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerEnter;
            enter.callback.AddListener(_ =>
                {
                    if (IsExist && boItem.SdItem.type == Define.Item.Type.Consume)
                        UseButton.gameObject.SetActive(true);
                });


            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener(_ => UseButton.gameObject.SetActive(false));

            var eventTrigger = gameObject.GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(enter);
            eventTrigger.triggers.Add(exit);
        }

        private void UsedItem()
        {
            boItem.ItemCount--;
            boItem.SdItem.ItemEffect();

            if (boItem.ItemCount == 0)
            {
                GameManager.User.boInventory.BoItems.Remove(boItem);
                boItem = null;
                IsExist = false;
            }

            RefreshSlot(boItem);
        }

        private Color SetHexColor(string hexCode)
        {
            Color color;

            if (ColorUtility.TryParseHtmlString(hexCode, out color))
            {
                return color;
            }
            else
            {
                Debug.Log($"컬러 변환 실패 : {hexCode}");
                return Color.white;
            }
        }

        private void DeActiveAll()
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}