
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
        bandit.Attack();
    }

    public override string GetName()
    {
        return "Attack";
    }
}
