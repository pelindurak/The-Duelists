
using UnityEngine;

public class StandGroundState : BaseState
{
    public StandGroundState(Bandit bandit) : base(bandit)
    {
    }

    public override void PreSwap()
    {
        bandit.Idle();
    }

    public override void PostSwap()
    {
    }

    public override void RunState()
    {
        bandit.StandGround();
    }

    public override string GetName()
    {
        return "StandGround";
    }
}
