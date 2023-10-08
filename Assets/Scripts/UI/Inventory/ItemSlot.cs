using ProjectW.DB;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectW
{
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
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
            get => isExist;
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

        private void SetSlot(BoItem boItem)
        {
            // 매개 변수로 받은 BoItem이 null이라는 것은 BoInventory의 해당 슬롯이 빈 슬롯이라는 의미
            if (boItem == null)
                return;

            IsExist = true;
            this.boItem = boItem;
            Icon.sprite = Resources.Load<Sprite>(boItem.SdItem.spritePath); // 아이템 아이콘
            Icon.color = SetHexColor(boItem.SdItem.colorCode);              // 아이콘 컬러
            CountText.text = boItem.ItemCount.ToString();                   // 아이템 갯수

            // 아이템의 아이콘과 갯수 UI 활성화
            Icon.gameObject.SetActive(true);
            ItemCount.SetActive(true);
        }

        // 아이템의 추가나 증가, 삭제가 일어났을 시 해당 슬롯의 아이템 정보를 업데이트
        public void RefreshSlot(BoItem boItem)
        {
            // 현재 슬롯에 이미 아이템이 존재한다면
            if (IsExist)
            {
                // 아이템의 갯수만 업데이트
                CountText.text = boItem.ItemCount.ToString();
                return;
            }

            // 슬롯을 매개 변수로 받은 아이템의 정보로 업데이트
            SetSlot(boItem);
        }

        private void AddUseButtonEvent()
        {
            UseButton.onClick.AddListener(UsedItem);

            EventTrigger.Entry enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            enter.callback.AddListener(_ =>
            {
                if (IsExist && boItem.SdItem.type == Define.Item.Type.Consume)
                    UseButton.gameObject.SetActive(true);
            });

            EventTrigger.Entry exit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
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
            return ColorUtility.TryParseHtmlString(hexCode, out var color) ? color : Color.white;
        }

        private void DeActiveAll()
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}