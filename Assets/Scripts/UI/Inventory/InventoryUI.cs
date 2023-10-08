using ProjectW.DB;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public class InventoryUI : MonoBehaviour
    {
        private const int slotCount = 16;

        public BoInventory boInventory;
        public Button CloseButton;
        public Transform SlotHolder;
        public ItemSlot[] itemSlots;

        public void Init()
        {
            // SlotHolder의 자식인 모든 아이템 슬롯을 들고 있는 itemSlots 배열 초기화
            itemSlots = new ItemSlot[slotCount];
            for (int i = 0; i < slotCount; i++)
                itemSlots[i] = SlotHolder.GetChild(i).GetComponent<ItemSlot>();
            
            // 실질적인 데이터를 들고 있는 BoInventory 할당 후 슬롯 정보 업데이트
            boInventory = GameManager.User.boInventory;
            for (int i = 0; i < boInventory.BoItems.Count; i++)
                itemSlots[i].RefreshSlot(boInventory.BoItems[i]);
            
            // 닫기 버튼 onClick이벤트 추가
            CloseButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }
}