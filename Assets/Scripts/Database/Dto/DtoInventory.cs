using ProjectW.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.DB
{
    [Serializable]
    public class DtoInventory : DtoBase
    {
        public List<DtoItem> DtoItems;

        public DtoInventory()
        {
            DtoItems = new List<DtoItem>();
        }
    }

    [Serializable]
    public class DtoItem
    {
        /// <summary>
        /// �ش� ������ ������ �ε���
        /// </summary>
        public int index;

        /// <summary>
        /// �ش� ������ ������ ����
        /// </summary>
        public int itemCount;
    }
}