
public class IdleState : BaseState
{
    public IdleState(Bandit bandit) : base(bandit)
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
        bandit.Idle();
    }

    public override string GetName()
    {
        return "IdleState";
    }

}
