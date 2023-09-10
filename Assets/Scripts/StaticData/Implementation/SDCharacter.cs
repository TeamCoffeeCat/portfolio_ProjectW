using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static ProjectW.Define.Actor;
using Newtonsoft.Json;

namespace ProjectW.SD
{
    [Serializable]
    public class SDCharacter : StaticData
    {
        /// <summary>
        /// 이름
        /// </summary>
        public string name;
        /// <summary>
        /// 일반 공격 타입
        /// </summary>
        public AttackType atkType;
        /// <summary>
        /// 최대 체력
        /// </summary>
        public float maxHp;
        /// <summary>
        /// 최대 마나
        /// </summary>
        public float maxMana;
        /// <summary>
        /// 최대 경험치
        /// </summary>
        public float maxExp;
        /// <summary>
        /// 공격력
        /// </summary>
        public float atk;
        /// <summary>
        /// 방어력
        /// </summary>
        public float def;
        /// <summary>
        /// 이동 속력
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// 점프 시 캐릭터에 가해지는 힘
        /// </summary>
        public float jumpForce;
        /// <summary>
        /// 일반 공격 범위
        /// </summary>
        public float atkRange;
        /// <summary>
        /// 일반 공격 간격 (쿨타임)
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// 성장스텟 테이블 인덱스 참조
        ///  -> 나중에 설명
        /// </summary>
        public int growthStatRef;
        /// <summary>
        /// 프리팹 경로
        /// </summary>
        public string resourcePath;
    }
}
