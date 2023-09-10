using ProjectW.Network;
using System;
using System.Linq;

namespace ProjectW.DB
{
    [Serializable]
    public class DtoCharacter : DtoBase
    {
        /// <summary>
        /// 유저의 캐릭터 인덱스
        /// </summary>
        public int index;

        /// <summary>
        /// 유저의 캐릭터 레벨
        /// </summary>
        public float level;

        /// <summary>
        /// 유저의 최대 경험치
        /// </summary>
        public float maxExp;

        /// <summary>
        /// 유저의 현재 경험치
        /// </summary>
        public float currentExp;

        public float maxHp;
        public float currentHp;
        public float maxMana;
        public float currentMana;
        public float atk;
        public float def;

        public DtoCharacter()
        {
            index = 1003;
            level = 1;
            currentExp = 0;
        }
    }
}
