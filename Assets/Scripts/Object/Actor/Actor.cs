using ProjectW.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ProjectW.Object
{
    /// <summary>
    /// �ΰ��� ������ ���̳����ϰ� �ൿ�ϴ� ��ü���� �߻�ȭ�� ���̽� Ŭ����
    /// ĳ����, ���� ��
    /// Actor�� �Ļ�Ŭ�������� ����Ǵ� ����� �ִ��� Actor�� ����
    /// �Ļ�Ŭ������ ���� �ٸ� ����� �ش� �Ļ�Ŭ�������� ����
    /// </summary>
    public abstract class Actor : MonoBehaviour
    {
        /// <summary>
        /// ������ bo ������ ����
        /// </summary>
        [HideInInspector]
        public BoActor boActor; 

        /// <summary>
        /// ���Ͱ� ���������� ����ϴ� ������Ʈ���� ����
        /// </summary>
        public Collider Coll { get; private set; }
        public Rigidbody rig;

        /// <summary>
        /// �ʱ�ȭ ��, �ܺο��� boActor �����͸� ���Թ޴´�.
        /// </summary>
        /// <param name="boActor"></param>
        public virtual void Initialize(BoActor boActor)
        {
            this.boActor = boActor;
        }

        protected virtual void Start()
        {
            // ���͵��� ����ϴ� ������Ʈ���� ������ �޴´�.
            Coll = GetComponent<Collider>();
            rig = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            MoveUpdate();
        }

        /// <summary>
        /// ���� ���� ���� �޼���
        /// </summary>
        public virtual void SetStats() { }

        /// <summary>
        /// �̵� ������Ʈ
        /// </summary>
        public virtual void MoveUpdate() { }
    }
}