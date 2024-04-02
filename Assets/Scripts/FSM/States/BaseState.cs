
public abstract class BaseState
{
    public Bandit bandit;

    public BaseState(Bandit bandit) 
    { 
        this.bandit = bandit;
    }

    public abstract void RunState();
    public abstract void PreSwap();
    public abstract void PostSwap();
    public abstract string GetName();

}
