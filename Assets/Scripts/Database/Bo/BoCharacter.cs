using ProjectW.SD;
using System;
using System.Linq;

namespace ProjectW.DB
{
    [Serializable]
    public class BoCharacter : BoActor
    {
        /// <summary>
        /// 유저의 최대 경험치
        /// </summary>
        public float maxExp;

        /// <summary>
        /// 유저의 현재 경험치
        /// </summary>
        public float currentExp;

        /// <summary>
        /// 캐릭터 기획 데이터
        /// </summary>
        public SDCharacter sdCharacter;

        /// <summary>
        /// 현재 땅에 있는지?
        /// </summary>
        public bool isGround;

        public BoCharacter(DtoCharacter dtoCharacter)
        {
            sdCharacter = GameManager.SD.sdCharacters.Where(sd => sd.index == dtoCharacter.index).SingleOrDefault();

            level = dtoCharacter.level;
            maxExp = dtoCharacter.maxExp;
            currentExp = dtoCharacter.currentExp;
            maxHp = dtoCharacter.maxHp;
            currentHp = dtoCharacter.currentHp;
            maxMana = dtoCharacter.maxMana;
            currentMana = dtoCharacter.currentMana;
            atk = dtoCharacter.atk;
            def = dtoCharacter.def;
        }
    }
}
