
public class AscendingLinearMF : BaseMF
{
    public float first { get; set; } = 0;
    public float second { get; set; } = 0;

    /// <param name="name">The name for the membership function.</param>
    /// <param name="first">Point where membership = 0</param>
    /// <param name="second">Point where membership = 1</param>
    public AscendingLinearMF(string name, float first, float second) : base(name)
    {
        this.first = first;
        this.second = second;
    }

    public override float GetMembership(float value)
    {
        if (value >= first && value <= second) return (value - first) / (second - first);
        else return 0;
    }

    public override float GetMin()
    {
        return first;
    }

    public override float GetMax()
    {
        return second;
    }
}
