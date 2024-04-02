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
        float xDiff = bandit.PlayerObject.transform.position.x - bandit.transform.position.x;
        float inputX = Mathf.Clamp(xDiff, -1f, 1f);
        bandit.Run(-inputX);
    }

    public override string GetName()
    {
        return "RunAwayState";
    }

}
