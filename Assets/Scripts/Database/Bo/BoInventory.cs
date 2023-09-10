using ProjectW.SD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectW.DB
{
    [Serializable]
    public class BoInventory
    {
        public int SlotCount;
        public List<BoItem> BoItems;

        public BoInventory(DtoInventory dtoInventory)
        {
            SlotCount = 16;
            BoItems = new List<BoItem>();
            foreach (DtoItem item in dtoInventory.DtoItems)
            {
                BoItems.Add(new BoItem(item));
            }
        }

        public bool IsExistItem(int itemindex)
        {
            return BoItems.Exists(boItem => boItem.SdItem.index.Equals(itemindex));
        }

        public bool IsFull()
        {
            if (BoItems.Count >= SlotCount)
                return true;
            else return false;
        }

        public void AddItem(int index)
        {
            // 인벤토리에 이미 존재하는 아이템 검색
            if (IsExistItem(index))
            {
                // TODO: Null
                var findedItem = BoItems.Find(i => i.SdItem.index.Equals(index));
                findedItem.ItemCount++; // 아이템 갯수 증가
                IngameManager.Instance.AddItemSystemMessage.Invoke(findedItem.SdItem.name);
                return;
            }

            // 못찾았을 때 새로운 아이템 생성
            var sdItem = GameManager.SD.sdItems.Where(sd => sd.index == index).SingleOrDefault();
            BoItems.Add(new BoItem(sdItem));
            IngameManager.Instance.AddItemSystemMessage.Invoke(sdItem.name);
        }

        private void CheckAllItem()
        {
            BoItems.ForEach(item =>
            {
                Debug.Log($"이름: {item.SdItem.name}, 수량: {item.ItemCount}");
            });
        }
    }

    [Serializable]
    public class BoItem
    {
        public SDItem SdItem;
        public int ItemCount;

        public BoItem(DtoItem dtoItem)
        {
            SdItem = GameManager.SD.sdItems.Where(item => item.index == dtoItem.index).SingleOrDefault();
            ItemCount = dtoItem.itemCount;
        }

        public BoItem(SDItem item)
        {
            SdItem = item;
            ItemCount = 1;
        }
    }
}