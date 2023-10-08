using ProjectW.SD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public bool IsExistItem(int itemIndex)
        {
            return BoItems.Exists(boItem => boItem.SdItem.index.Equals(itemIndex));
        }

        public bool IsFull()
        {
            return BoItems.Count >= SlotCount;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void AddItem(int index)
        {
            // 인벤토리에 이미 존재하는 아이템인지 검색
            if (IsExistItem(index))
            {
                var findedItem = BoItems.Find(item => item.SdItem.index.Equals(index));
                findedItem.ItemCount++; // 아이템 갯수 증가
                IngameManager.Instance.AddItemSystemMessage.Invoke(findedItem.SdItem.name); // 아이템 획득 시스템 메세지
                return;
            }

            // 인벤토리에 존재하지 않는 아이템이라면 새로운 아이템 생성
            var sdItem = GameManager.SD.sdItems.SingleOrDefault(sd => sd.index == index);
            BoItems.Add(new BoItem(sdItem)); // 인벤토리의 아이템 List에 추가
            IngameManager.Instance.AddItemSystemMessage.Invoke(sdItem.name); // 아이템 획득 시스템 메세지
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

        // 서버에 저장 된 데이터가 있을 경우 Dto 데이터를 기반으로 boItem을 생성
        public BoItem(DtoItem dtoItem)
        {
            SdItem = GameManager.SD.sdItems.SingleOrDefault(item => item.index == dtoItem.index);
            ItemCount = dtoItem.itemCount;
        }

        // 새로운 아이템을 생성 할 경우 Sd 데이터를 기반으로 boItem을 생성
        public BoItem(SDItem item)
        {
            SdItem = item;
            ItemCount = 1;
        }
    }
}