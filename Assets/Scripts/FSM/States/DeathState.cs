
public class DeathState : BaseState
{
    public DeathState(Bandit bandit) : base(bandit)
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
        bandit.Death();
    }

    public override string GetName()
    {
        return "Death";
    }
}
