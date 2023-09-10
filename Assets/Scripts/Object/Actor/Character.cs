using ProjectW.DB;
using UnityEngine;
using System.Collections;
using CoffeeCat.Simplify;
using System.Linq;

namespace ProjectW.Object
{
    public class Character : Actor
    {
        public CharacterStateMachine stateMachine;
        public BoCharacter boCharacter;
        public Transform AttackRangeCenter;

        private Transform mesh;
        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;
        public bool onHit = false;

        public float CurrentHp
        {
            get { return boCharacter.currentHp; }
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
            get { return boCharacter.currentExp; }
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

        public override void MoveUpdate()
        {
            Vector3 direction = boCharacter.moveDir;
            Transform mainCamTr = Camera.main.transform;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamTr.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                mesh.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                transform.Translate(moveDirection * boActor.moveSpeed * Time.deltaTime);
            }
        }

        public bool IsMove()
        {
            if (boCharacter.moveDir.magnitude < 0.1f)
                return false;
            else
                return true;
        }

        public bool IsGround()
        {
            if (boCharacter.isGround)
                return true;
            else
                return false;
        }

        public bool IsDead()
        {
            if (CurrentHp <= 0)
                return true;
            else
                return false;
        }

        public void OnJump()
        {
            boCharacter.isGround = false;
            rig.AddForce(Vector3.up * boCharacter.sdCharacter.jumpForce, ForceMode.VelocityChange);
        }

        public void OnAttack()
        {
            var monsters = Physics.OverlapSphere(AttackRangeCenter.position, boCharacter.atkRange, 1 << LayerMask.NameToLayer("Monster"));

            foreach (var monster in monsters)
            {
                if (monster.TryGetComponent(out Monster mon))
                {
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
            var growthStat = GameManager.SD.sdGrowthStat.Where(stat => stat.index == boCharacter.sdCharacter.index).SingleOrDefault();

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