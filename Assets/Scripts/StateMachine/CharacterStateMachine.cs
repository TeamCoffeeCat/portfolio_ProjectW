using System.Diagnostics.CodeAnalysis;
using ProjectW.Object;
using UnityEngine;
using CoffeeCat;
using State = ProjectW.Define.Actor.State;

[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
[SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
public class CharacterStateMachine : StateMachine
{
    private Character character;

    private float attackInterval = 0f;
    private float currentAttackInterval = 0f;

    private const KeyCode jump = KeyCode.Space;
    private const KeyCode attack = KeyCode.Mouse0;
    private readonly int walkAnimHash = Animator.StringToHash("IsWalk");
    private readonly int jumpAnimHash = Animator.StringToHash("JumpTrigger");
    private readonly int attackAnimHash = Animator.StringToHash("AttackTrigger");
    private readonly int hitAnimHash = Animator.StringToHash("HitTrigger");
    private readonly int dieAnimHash = Animator.StringToHash("DieTrigger");

    private bool finishAttack = false;
    private bool finishHit = false;

    protected override void Start()
    {
        base.Start();
        character = actor as Character;
        attackInterval = character.boCharacter.atkInterval;
        currentAttackInterval = attackInterval;

        StateChange(State.Idle);
    }

    protected override void Update()
    {
        base.Update();
        currentAttackInterval += Time.deltaTime;
    }

    protected override void Idle_Enter()
    {
        anim.SetBool(walkAnimHash, false);
    }

    protected override void Idle_Update()
    {
        if (character.IsMove())
            StateChange(State.Walk);
        else if (Input.GetKeyDown(jump))
            StateChange(State.Jump);
        else if (Input.GetKey(attack))
        {
            if (currentAttackInterval >= attackInterval)
                StateChange(State.Attack);
        }
    }

    protected override void Idle_Exit()
    {

    }

    protected override void Walk_Enter()
    {
        anim.SetBool(walkAnimHash, true);
    }

    protected override void Walk_Update()
    {
        if (!character.IsMove())
            StateChange(State.Idle);
        else if (Input.GetKeyDown(jump))
            StateChange(State.Jump);
        else if (Input.GetKey(attack))
        {
            if (currentAttackInterval >= attackInterval)
                StateChange(State.Attack);
        }
    }

    protected override void Walk_Exit()
    {

    }

    protected override void Jump_Enter()
    {
        anim.SetTrigger(jumpAnimHash);
        character.OnJump();
    }

    protected override void Jump_Update()
    {
        if (character.IsGround())
        {
            if (!character.IsMove())
                StateChange(State.Idle);
            else
                StateChange(State.Walk);
        }
    }

    protected override void Jump_Exit()
    {

    }

    protected override void Attack_Enter()
    {
        anim.SetTrigger(attackAnimHash);
        character.OnAttack();
    }

    protected override void Attack_Update()
    {
        if (finishAttack)
        {
            if (!character.IsMove())
                StateChange(State.Idle);
            else
                StateChange(State.Walk);
        }
    }

    protected override void Attack_Exit()
    {
        currentAttackInterval = 0;
        finishAttack = false;
    }

    protected override void Hit_Enter()
    {
        anim.SetTrigger(hitAnimHash);
    }

    protected override void Hit_Update()
    {
        if (character.IsDead())
        {
            StateChange(State.Die);
        }

        if (finishHit)
        {
            if (!character.IsMove())
                StateChange(State.Idle);
            else if (character.IsMove())
                StateChange(State.Walk);
            else if (Input.GetKey(attack))
            {
                if (currentAttackInterval >= attackInterval)
                    StateChange(State.Attack);
            }
        }
    }

    protected override void Hit_Exit()
    {
        finishHit = false;
    }

    protected override void Die_Enter()
    {
        anim.SetTrigger(dieAnimHash);
        StartCoroutine(character.OnDead());
    }

    protected override void Die_Update()
    {

    }

    protected override void Die_Exit()
    {

    }

    public void AnimEvent_FinishAttack()
    {
        finishAttack = true;
    }

    public void AnimEvent_FinishHit()
    {
        finishHit = true;
        character.onHit = false;
    }
}
