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
        /// 해당 슬롯의 아이템 인덱스
        /// </summary>
        public int index;

        /// <summary>
        /// 해당 슬롯의 아이템 갯수
        /// </summary>
        public int itemCount;
    }
}