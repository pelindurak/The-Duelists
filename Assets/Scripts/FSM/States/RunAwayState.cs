using UnityEngine;

public class RunAwayState : BaseState
{
    public RunAwayState(Bandit bandit) : base(bandit)
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
        bandit.RunAway();
    }

    public override string GetName()
    {
        return "RunAway";
    }

}
