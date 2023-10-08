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
            // �κ��丮�� �̹� �����ϴ� ���������� �˻�
            if (IsExistItem(index))
            {
                var findedItem = BoItems.Find(item => item.SdItem.index.Equals(index));
                findedItem.ItemCount++; // ������ ���� ����
                IngameManager.Instance.AddItemSystemMessage.Invoke(findedItem.SdItem.name); // ������ ȹ�� �ý��� �޼���
                return;
            }

            // �κ��丮�� �������� �ʴ� �������̶�� ���ο� ������ ����
            var sdItem = GameManager.SD.sdItems.SingleOrDefault(sd => sd.index == index);
            BoItems.Add(new BoItem(sdItem)); // �κ��丮�� ������ List�� �߰�
            IngameManager.Instance.AddItemSystemMessage.Invoke(sdItem.name); // ������ ȹ�� �ý��� �޼���
        }

        private void CheckAllItem()
        {
            BoItems.ForEach(item =>
            {
                Debug.Log($"�̸�: {item.SdItem.name}, ����: {item.ItemCount}");
            });
        }
    }

    [Serializable]
    public class BoItem
    {
        public SDItem SdItem;
        public int ItemCount;

        // ������ ���� �� �����Ͱ� ���� ��� Dto �����͸� ������� boItem�� ����
        public BoItem(DtoItem dtoItem)
        {
            SdItem = GameManager.SD.sdItems.SingleOrDefault(item => item.index == dtoItem.index);
            ItemCount = dtoItem.itemCount;
        }

        // ���ο� �������� ���� �� ��� Sd �����͸� ������� boItem�� ����
        public BoItem(SDItem item)
        {
            SdItem = item;
            ItemCount = 1;
        }
    }
}