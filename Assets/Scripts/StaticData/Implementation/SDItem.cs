using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using UnityEngine;

namespace ProjectW.SD
{
    [Serializable]
    public class SDItem: StaticData
    {
        /// <summary>
        /// �̸�
        /// </summary>
        public string name;

        /// <summary>
        /// ������ Ÿ�� ��з�
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Define.Item.Type type;

        /// <summary>
        /// ������ Ÿ�� �Һз�
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Define.Item.SubType subType;

        /// <summary>
        /// ������ ȿ�� ��ġ
        /// </summary>
        public float value;

        /// <summary>
        /// ������ ���
        /// </summary>
        public string prefabPath;

        /// <summary>
        /// ���������� ����� sprite ���
        /// </summary>
        public string spritePath;

        /// <summary>
        /// ���������� ����� sprite �÷��ڵ�
        /// </summary>
        public string colorCode;

        public virtual void ItemEffect() { }

        public SDItem() { }

        public SDItem(SDItem origin) 
        {
            index = origin.index;
            name = origin.name;
            type = origin.type;
            subType = origin.subType;
            value = origin.value;
            prefabPath = origin.prefabPath;
            spritePath = origin.spritePath;
            colorCode = origin.colorCode;
        }
    }
}