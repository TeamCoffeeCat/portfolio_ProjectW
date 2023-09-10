using UnityEngine;
using CoffeeCat;
using ProjectW.Object;
using static ProjectW.Define.Actor;

public class MonsterStateMachine : StateMachine
{
    private Monster monster;

    private float findPlayerInterval = 0.25f;
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

    protected override void Idle_Enter()
    {
        anim.SetBool(walkAnimHash, false);
    }

    protected override void Idle_Update()
    {
        // Find Player
        if (findPlayerInterval <= currentFindInterval)
        {
            if (monster.IsPlayerInTraceDistance())
            {
                StateChange(State.Walk);
            }
        }
    }

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

    protected override void Attack_Enter()
    {
        anim.SetTrigger(attackAnimHash);
        monster.OnAttack();
    }

    protected override void Attack_Update()
    {
        // normalzied Time = 0f ~ 1f
        // if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        if (finishAttack)
        {
            if (monster.IsPlayerInTraceDistance())
            {
                StateChange(State.Walk);
            }
            else
            {
                StateChange(State.Idle);
            }
        }
    }

    protected override void Attack_Exit()
    {
        currentAttackInterval = 0f;
        finishAttack = false;
    }

    protected override void Hit_Enter()
    {
        anim.SetTrigger(hitAnimHash);
    }

    protected override void Hit_Update()
    {
        if (monster.IsDead())
        {
            StateChange(State.Die);
        }

        if (finishHit)
        {
            if (monster.IsPlayerInTraceDistance())
            {
                StateChange(State.Walk);
            }
            else
            {
                StateChange(State.Idle);
            }
        }
    }

    protected override void Hit_Exit()
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
