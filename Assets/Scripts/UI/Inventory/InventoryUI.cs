using ProjectW.DB;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW
{
    public class InventoryUI : MonoBehaviour
    {
        private int slotCount = 16;

        public BoInventory boInventory;
        public Button CloseButton;
        public Transform SlotHolder;
        public ItemSlot[] itemSlots;

        public void Init()
        {
            itemSlots = new ItemSlot[slotCount];
            for (int i = 0; i < slotCount; i++)
                itemSlots[i] = SlotHolder.GetChild(i).GetComponent<ItemSlot>();
            
            boInventory = GameManager.User.boInventory;
            for (int i = 0; i < boInventory.BoItems.Count; i++)
                itemSlots[i].RefreshSlot(boInventory.BoItems[i]);
            
            CloseButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }
}