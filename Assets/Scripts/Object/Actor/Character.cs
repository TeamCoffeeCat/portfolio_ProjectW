using ProjectW.DB;
using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using CoffeeCat.Simplify;
using System.Linq;

namespace ProjectW.Object
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public class Character : Actor
    {
        public CharacterStateMachine stateMachine;
        public BoCharacter boCharacter;
        public Transform AttackRangeCenter;

        private Transform mesh;
        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;
        private int multiAttackCount = 5;
        public bool onHit = false;

        private float CurrentHp
        {
            get => boCharacter.currentHp;
            set
            {
                boCharacter.currentHp = value;
                if (boCharacter.currentHp <= 0)
                {
                    boCharacter.currentHp = 0;
                }
                IngameManager.Instance.playerHpValueChanged.Invoke(boCharacter.currentHp);
            }
        }

        public float CurrentExp
        {
            get => boCharacter.currentExp;
            set
            {
                boCharacter.currentExp = value;
                IngameManager.Instance.playerExpValueChanged.Invoke(boCharacter.currentExp);
                if (boCharacter.currentExp >= boCharacter.maxExp)
                {
                    var restExp = boCharacter.currentExp - boCharacter.maxExp;
                    CharacterLevelUp(restExp);
                }
            }
        }

        protected override void FixedUpdate()
        {
            if (stateMachine.state == Define.Actor.State.Die)
                return;

            MoveUpdate();
            FellFromStage();
        }

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);
            mesh = transform.GetChild(0);
            boCharacter = boActor as BoCharacter;
            stateMachine = GetComponent<CharacterStateMachine>();
            SetStats();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void InitStat()
        {
            boCharacter.atkType = boCharacter.sdCharacter.atkType;
            boCharacter.maxHp = boCharacter.sdCharacter.maxHp;
            boCharacter.currentHp = boCharacter.sdCharacter.maxHp;
            boCharacter.maxMana = boCharacter.sdCharacter.maxMana;
            boCharacter.currentMana = boCharacter.sdCharacter.maxMana;
            boCharacter.maxExp = boCharacter.sdCharacter.maxExp;
            boCharacter.atk = boCharacter.sdCharacter.atk;
            boCharacter.def = boCharacter.sdCharacter.def;
        }

        public override void SetStats()
        {
            boCharacter.type = Define.Actor.Type.Character;
            boCharacter.moveSpeed = boCharacter.sdCharacter.moveSpeed;
            boCharacter.atkInterval = boCharacter.sdCharacter.atkInterval;
            boCharacter.atkRange = boCharacter.sdCharacter.atkRange;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public override void MoveUpdate()
        {
            Vector3 direction = boCharacter.moveDir; // 캐릭터의 방향 벡터
            Transform mainCamTr = Camera.main.transform; // 메인 카메라 위치

            // 캐릭터의 방향 벡터의 크기가 0.1보다 크다면 로직을 실행
            if (direction.magnitude >= 0.1f)
            {
                // 방향 벡터의 라디안 각도를 구하고 이를 디그리로 변환
                // 카메라가 바라보는 각을 기준으로 이동하기 위해 카메라의 y각을 사용
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamTr.eulerAngles.y;
                // 자연스러운 캐릭터의 회전을 위한 것으로 SmoothDampAngle을 통해 절차적 회전 각도를 구함 
                float angle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                // 캐릭터 게임 오브젝트의 회전과 메쉬 오브젝트 회전을 분리
                mesh.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                // 기획데이터에서 정의한 이동속도와 deltaTime을 이용하여 캐릭터의 위치를 업데이트
                transform.Translate(moveDirection * (boActor.moveSpeed * Time.deltaTime));
            }
        }

        public bool IsMove()
        {
            return !(boCharacter.moveDir.magnitude < 0.1f);
        }

        public bool IsGround()
        {
            return boCharacter.isGround;
        }

        public bool IsDead()
        {
            return CurrentHp <= 0;
        }

        public void OnJump()
        {
            boCharacter.isGround = false;
            rig.AddForce(Vector3.up * boCharacter.sdCharacter.jumpForce, ForceMode.VelocityChange);
        }

        public void OnAttack()
        {
            var monsters = new Collider[multiAttackCount];
            // 임의의 구체의 크기만큼 범위를 설정하고 범위 내의 몬스터 Collider를 검출하여 배열에 저장
            Physics.OverlapSphereNonAlloc(AttackRangeCenter.position, boCharacter.atkRange, monsters, 1 << LayerMask.NameToLayer("Monster"));
            
            foreach (var monster in monsters)
            {
                if (!monster)
                    return;
                
                // Monster 컴포넌트의 액세스에 성공했다면
                if (monster.TryGetComponent(out Monster mon))
                {
                    // 몬스터 피격 처리
                    mon.OnHit(boCharacter.atk);
                }
            }
        }

        public void OnHit(float damage)
        {
            if (stateMachine.state == Define.Actor.State.Die)
                return;

            if (onHit)
                return;

            onHit = true;

            damage = damage - boCharacter.def;
            if (damage < 0)
                damage = 0;

            stateMachine.StateChange(Define.Actor.State.Hit);
            PoolManagerLight.Instance.SpawnEffect("Character_Hit", mesh.position, Quaternion.identity, 0.5f);
            CurrentHp -= damage;
        }

        public IEnumerator OnDead()
        {
            yield return new WaitForSeconds(2f);

            UIPresenter.Instance.ActiveRespawnMessage();
        }

        private void CharacterLevelUp(float restExp)
        {
            var growthStat = GameManager.SD.sdGrowthStat.SingleOrDefault(stat => stat.index == boCharacter.sdCharacter.index);

            PoolManagerLight.Instance.SpawnEffect("Character_LevelUp", mesh.position + new Vector3(0, 0.5f, 0), Quaternion.identity, 0.5f);
            boCharacter.level++;
            boCharacter.maxHp += growthStat.maxHp;
            boCharacter.maxMana += growthStat.maxMana;
            boCharacter.atk += growthStat.atk;
            boCharacter.def += growthStat.def;
            boCharacter.maxExp = boCharacter.maxExp * growthStat.maxExp;

            boCharacter.currentHp = boCharacter.maxHp;
            boCharacter.currentMana = boCharacter.maxMana;
            boCharacter.currentExp = restExp;

            IngameManager.Instance.playerHpValueChanged.Invoke(boCharacter.currentHp);
            IngameManager.Instance.playerMpValueChanged.Invoke(boCharacter.currentMana);
            IngameManager.Instance.playerExpValueChanged.Invoke(boCharacter.currentExp);
            IngameManager.Instance.playerLevelValueChanged.Invoke(boCharacter.level);
        }

        private void FellFromStage()
        {
            if (transform.position.y <= -30f)
            {
                IngameManager.Instance.RespawnCharacter();
            }
        }
    }
}