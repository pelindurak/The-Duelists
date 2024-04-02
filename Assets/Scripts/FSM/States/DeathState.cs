
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
        bandit.m_animator.SetTrigger("Death");
        bandit._isDead = true;
    }

    public override string GetName()
    {
        return "DeathState";
    }
}
