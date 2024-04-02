using UnityEngine;

public class ChasePlayerState : BaseState
{
    public ChasePlayerState(Bandit bandit) : base(bandit)
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
        float inputX = 0f;
        if (!bandit.IsCloseToPlayer())
        {
            float xDiff = bandit.PlayerObject.transform.position.x - bandit.transform.position.x;
            inputX = Mathf.Clamp(xDiff, -1f, 1f);
            bandit.Run(inputX);
        }
    }

    public override string GetName()
    {
        return "ChasePlayerState";
    }
}
