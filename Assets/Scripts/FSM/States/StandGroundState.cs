
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
        bandit.SwapSpriteDirection(bandit.PlayerObject.transform.position.x - bandit.transform.position.x);
    }

    public override string GetName()
    {
        return "StandGround";
    }
}
