

public class DescendingLinearMF : BaseMF
{
    public float first { get; set; } = 0;
    public float second { get; set; } = 0;

    /// <param name="name">The name for the membership function.</param>
    /// <param name="first">Point where membership = 1</param>
    /// <param name="second">Point where membership = 0</param>
    public DescendingLinearMF(string name, float first, float second) : base(name)
    {
        this.first = first;
        this.second = second;
    }

    public override float GetMembership(float value)
    {
        if (value >= first && value <= second) return (value - second) / (first - second);
        else return 0;
    }

    public override float GetMin()
    {
        return second;
    }

    public override float GetMax()
    {
        return first;
    }
}
