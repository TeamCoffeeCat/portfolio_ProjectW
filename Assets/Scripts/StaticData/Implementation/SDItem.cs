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
        /// 이름
        /// </summary>
        public string name;

        /// <summary>
        /// 아이템 타입 대분류
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Define.Item.Type type;

        /// <summary>
        /// 아이템 타입 소분류
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Define.Item.SubType subType;

        /// <summary>
        /// 아이템 효과 수치
        /// </summary>
        public float value;

        /// <summary>
        /// 프리팹 경로
        /// </summary>
        public string prefabPath;

        /// <summary>
        /// 아이콘으로 사용할 sprite 경로
        /// </summary>
        public string spritePath;

        /// <summary>
        /// 아이콘으로 사용할 sprite 컬러코드
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