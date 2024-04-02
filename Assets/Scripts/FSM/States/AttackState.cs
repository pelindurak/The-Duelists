using System.Collections;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(Bandit bandit) : base(bandit)
    {
    }

    public override void PreSwap()
    {
    }

    public override void PostSwap()
    {
    }

    public override void RunState()
    {
        bandit.ResetAttackTimer();

        bandit.m_animator.SetTrigger("Attack");
        bandit.StartCoroutine(bandit.Damage());
    }

    public override string GetName()
    {
        return "AttackState";
    }
}
