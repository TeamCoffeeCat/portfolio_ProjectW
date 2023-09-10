using ProjectW.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ProjectW.Object
{
    /// <summary>
    /// 인게임 내에서 다이나믹하게 행동하는 객체들의 추상화된 베이스 클래스
    /// 캐릭터, 몬스터 등
    /// Actor의 파생클래스에서 공통되는 기능은 최대한 Actor에 정의
    /// 파생클래스에 따라 다른 기능은 해당 파생클래스에서 정의
    /// </summary>
    public abstract class Actor : MonoBehaviour
    {
        /// <summary>
        /// 액터의 bo 데이터 참조
        /// </summary>
        [HideInInspector]
        public BoActor boActor; 

        /// <summary>
        /// 액터가 공통적으로 사용하는 컴포넌트들의 참조
        /// </summary>
        public Collider Coll { get; private set; }
        public Rigidbody rig;

        /// <summary>
        /// 초기화 시, 외부에서 boActor 데이터를 주입받는다.
        /// </summary>
        /// <param name="boActor"></param>
        public virtual void Initialize(BoActor boActor)
        {
            this.boActor = boActor;
        }

        protected virtual void Start()
        {
            // 액터들이 사용하는 컴포넌트들의 참조를 받는다.
            Coll = GetComponent<Collider>();
            rig = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            MoveUpdate();
        }

        /// <summary>
        /// 액터 스텟 설정 메서드
        /// </summary>
        public virtual void SetStats() { }

        /// <summary>
        /// 이동 업데이트
        /// </summary>
        public virtual void MoveUpdate() { }
    }
}