using CoffeeCat.Simplify;
using ProjectW.DB;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectW.Object
{
    public class Monster : Actor
    {
        public int index;
        public BoMonster boMonster;
        public RectTransform hpBar;
        private MonsterStateMachine stateMachine;
        private NavMeshAgent navMeshAgent;
        private Vector3 hitEffectOffset;

        protected override void Start()
        {
            base.Start();
            Initialize();
        }

        public void Initialize()
        {
            var sd = GameManager.SD.sdMonster.Where(mon => mon.index == index).SingleOrDefault();
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
            navMeshAgent.isStopped = true;

            if (Physics.CheckSphere(transform.position, boMonster.atkRange, 1 << LayerMask.NameToLayer("Player")))
            {
                IngameManager.Instance.character.OnHit(boMonster.atk);
            }
        }

        /// <summary>
        /// 플레이어를 공격이 가능한 상태인지 체크
        /// </summary>
        /// <returns></returns>
        public bool IsAttackable()
        {
            return (navMeshAgent.remainingDistance <= boMonster.atkRange);
        }


        public void OnHit(float damage)
        {
            damage = damage - boMonster.def;
            if (damage < 0)
                damage = 0;

            boMonster.currentHp -= damage;
            HpBarRefresh();
            stateMachine.StateChange(Define.Actor.State.Hit);

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
            item.GetComponent<Item>().SetIndex(1000);
            // 어떤 아이템을 드랍할지?
            // 아이템 Type마다 이펙트 색 다르게 할지?
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
