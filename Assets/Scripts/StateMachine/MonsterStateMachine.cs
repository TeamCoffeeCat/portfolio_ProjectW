using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using CoffeeCat;
using ProjectW.Object;
using static ProjectW.Define.Actor;

[SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
public class MonsterStateMachine : StateMachine
{
    private Monster monster;

    private float findPlayerInterval = 0.5f;
    private float currentFindInterval = 0f;

    private float attackInterval = 0f;
    private float currentAttackInterval = 0f;

    private readonly int walkAnimHash = Animator.StringToHash("IsWalk");
    private readonly int attackAnimHash = Animator.StringToHash("AttackTrigger");
    private readonly int hitAnimHash = Animator.StringToHash("HitTrigger");

    private bool finishAttack = false;
    private bool finishHit = false;

    protected override void Start()
    {
        base.Start();
        monster = actor as Monster;
        attackInterval = monster.boMonster.atkInterval;
        currentAttackInterval = attackInterval;

        StateChange(State.Idle);
    }

    protected override void Update()
    {
        base.Update();
        currentFindInterval += Time.deltaTime;
        currentAttackInterval += Time.deltaTime;
    }

    // Idle 상태 진입 시 1회 호출
    protected override void Idle_Enter()
    {
        anim.SetBool(walkAnimHash, false);
    }

    // Idle 상태에서 프레임마다 호출
    protected override void Idle_Update()
    {
        // 일정 시간마다 플레이어의 위치를 탐색
        if (findPlayerInterval <= currentFindInterval)
        {
            // 플레이어의 위치를 탐색 가능한 거리라면
            if (monster.IsPlayerInTraceDistance())
            {
                StateChange(State.Walk);
            }
        }
    }

    // Idle 상태 탈출 시 1회 호출
    protected override void Idle_Exit()
    {
        currentFindInterval = 0f;
    }

    protected override void Walk_Enter()
    {
        anim.SetBool(walkAnimHash, true);
        monster.StartTrace();
    }

    protected override void Walk_Update()
    {
        // Check Attackable Player
        if (attackInterval <= currentAttackInterval)
        {
            if (monster.IsAttackable())
            {
                StateChange(State.Attack);
            }
        }
        else
        {
            StateChange(State.Idle);
        }

        // Player Lost Restore To Idle
        if (!monster.IsPlayerInTraceDistance())
        {
            StateChange(State.Idle);
        }
    }

    protected override void Walk_Exit()
    {
        monster.NavMeshStop();
    }

    protected override void Attack_Enter() // 공격 상태 진입 시 1회 호출
    {
        anim.SetTrigger(attackAnimHash);
        monster.OnAttack();
    }

    protected override void Attack_Update() // 공격 상태 시 매 프레임 호출
    {
        if (finishAttack)
        {
            StateChange(monster.IsPlayerInTraceDistance() ? State.Walk : State.Idle);
        }
    }

    protected override void Attack_Exit() // 공격 상태에서 다른 상태로 변경 시 1회 호출
    {
        currentAttackInterval = 0f;
        finishAttack = false;
    }

    protected override void Hit_Enter() // 피격 상태 진입 시 1회 호출
    {
        anim.SetTrigger(hitAnimHash);
    }

    protected override void Hit_Update() // 피격 상태 시 매 프레임 호출
    {
        if (monster.IsDead())
        {
            StateChange(State.Die);
        }

        if (finishHit)
        {
            StateChange(monster.IsPlayerInTraceDistance() ? State.Walk : State.Idle);
        }
    }

    protected override void Hit_Exit() // 피격 상태에서 다른 상태로 변경 시 1회 호출
    {
        finishHit = false;
    }

    protected override void Die_Enter()
    {
        monster.OnDead();
    }

    protected override void Die_Update()
    {

    }

    protected override void Die_Exit()
    {

    }

    #region AnimEvent
    public void AnimEvent_FinishAttack()
    {
        finishAttack = true;
    }

    public void AnimEvent_FinishHit()
    {
        finishHit = true;
    }
    #endregion
}
