using ProjectW.SD;
using System;

namespace ProjectW.DB
{
    [Serializable]
    public class BoMonster : BoActor
    {
        public SDMonster sdMonster;

        /// <summary>
        /// 얻을 수 있는 경험치
        /// </summary>
        public float getExp;

        /// <summary>
        /// 현재 생성자에 파라미터로 sd 데이터를 받는 형태만 존재함
        ///  -> dto를 받는 형태는 존재하지 않음
        ///     이게 의미하는 바는, 몬스터 데이터의 생성을 서버가 제어하지 않는다는 것
        ///     이 말은 클라이언트에서 몬스터를 생성하겠다는 뜻
        /// </summary>
        /// <param name="sdMonster"></param>
        public BoMonster(SDMonster sdMonster)
        {
            this.sdMonster = sdMonster;

            type = Define.Actor.Type.Monster;
            atkType = sdMonster.atkType;
            moveSpeed = sdMonster.moveSpeed;
            currentHp = sdMonster.maxHp;
            maxHp = sdMonster.maxHp;
            currentMana = sdMonster.maxMana;
            maxMana = sdMonster.maxMana;
            atk = sdMonster.atk;
            getExp = sdMonster.getExp;
            atkRange = sdMonster.atkRange;
            atkInterval = sdMonster.atkInterval;
        }
    }
}
