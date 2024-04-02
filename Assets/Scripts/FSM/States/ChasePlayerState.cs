
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
        bandit.ChasePlayer();
    }

    public override string GetName()
    {
        return "ChasePlayer";
    }
}
