using System.Diagnostics.CodeAnalysis;
using CoffeeCat.Simplify;
using ProjectW.DB;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectW.Object
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public class Monster : Actor
    {
        public int index;
        public BoMonster boMonster;
        public RectTransform hpBar;
        private MonsterStateMachine stateMachine;
        private NavMeshAgent navMeshAgent;
        private Vector3 hitEffectOffset;

        private bool test;

        protected override void Start()
        {
            base.Start();
            Initialize();
        }
        
        private void Initialize()
        {
            var sd = GameManager.SD.sdMonster.SingleOrDefault(mon => mon.index == index);
            boActor = boMonster = new BoMonster(sd);
            stateMachine = GetComponent<MonsterStateMachine>();
            hitEffectOffset = new Vector3(0, 0.2f, 0.15f);

            HpBarRefresh();
            InitNavMeshAgent();
        }

        public override void SetStats()
        {
            boMonster.currentHp = boMonster.sdMonster.maxHp;
            boMonster.currentMana = boMonster.sdMonster.maxMana;
            HpBarRefresh();
        }

        public void OnAttack()
        {
            // 플레이어를 추적하다가 공격범위에 들어오면 즉시 추적을 멈추고 그 자리에서 공격
            navMeshAgent.isStopped = true;

            // 공격 범위에 들어온 충돌체가 플레이어인지 검사
            if (Physics.CheckSphere(transform.position, boMonster.atkRange, 1 << LayerMask.NameToLayer("Player")))
            {
                // 플레이어라면 플레이어의 피격 함수 실행
                IngameManager.Instance.character.OnHit(boMonster.atk);
            }
        }

        // 플레이어를 공격이 가능한 상태인지 체크
        public bool IsAttackable()
        {
            return (navMeshAgent.remainingDistance <= boMonster.atkRange);
        }

        public void OnHit(float damage)
        {
            damage = damage - boMonster.def;
            if (damage < 0)
                damage = 0;

            boMonster.currentHp -= damage;                    // 몬스터의 체력에 데미지 처리
            HpBarRefresh();                                   // 변경 된 체력에 따라 Hp바 업데이트
            stateMachine.StateChange(Define.Actor.State.Hit); // 몬스터의 상태를 피격 상태로 변경

            // Pool에서 몬스터의 피격 이펙트를 꺼내 활성화(딜레이 후 자동으로 디스폰되는 이펙트용 스폰 함수)
            PoolManagerLight.Instance.SpawnEffect("Monster_Hit", transform.position + hitEffectOffset, Quaternion.identity, 0.3f);
        }

        public bool IsDead()
        {
            return boMonster.currentHp <= 0;
        }

        public void OnDead()
        {
            DropItem();
            PoolManagerLight.Instance.SpawnEffect(Define.Effect.Monster.Monster_Despawn.ToString(), transform.position, Quaternion.identity, 2);
            PoolManagerLight.Instance.Despawn(gameObject);

            IngameManager.Instance.character.CurrentExp += boMonster.getExp;
        }

        private void DropItem()
        {
            var item = PoolManagerLight.Instance.SpawnToPool("Item", transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity);
            var randomIndex = Random.Range(1000, 1004);
            item.GetComponent<Item>().SetIndex(randomIndex);
        }

        public void StartTrace()
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(IngameManager.Instance.character.transform.position);
        }

        public bool IsPlayerInTraceDistance()
        {
            var distance = (IngameManager.Instance.character.transform.position - transform.position).sqrMagnitude;
            return distance <= boMonster.sdMonster.detectionRange;
        }

        public void NavMeshStop()
        {
            navMeshAgent.isStopped = true;
        }

        private void InitNavMeshAgent()
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.radius = 0.3f;
            navMeshAgent.height = 0.5f;
            navMeshAgent.acceleration = 1.5f;
            navMeshAgent.angularSpeed = 500f;
            navMeshAgent.speed = boMonster.moveSpeed;
            navMeshAgent.stoppingDistance = boMonster.atkRange;

            navMeshAgent.isStopped = true;
        }

        private void HpBarRefresh()
        {
            var width = (boMonster.currentHp / boMonster.maxHp) * 100f;
            hpBar.sizeDelta = new Vector2(width, hpBar.sizeDelta.y);
        }
    }
}
