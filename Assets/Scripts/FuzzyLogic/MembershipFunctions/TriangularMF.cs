using System;

public class TriangularMF : BaseMF
{
    public float first { get; set; } = 0;
    public float second { get; set; } = 0;
    public float third { get; set; } = 0;

    /// <param name="name">The name for the membership function.</param>
    /// <param name="first">Increasing point</param>
    /// <param name="second">Point where membership = 1</param>
    /// <param name="third">Descend point</param>
    public TriangularMF(string name, float first, float second, float third) : base(name)
    {
        this.first = first;
        this.second = second;
        this.third = third;
    }

    public override float GetMembership(float value)
    {
        if (value >= second && value < third) return (third - value) / (third - second);
        else if (value > first && value < second) return (value - first) / (second - first);
        else return 0;
    }

    public override float GetMin()
    {
        return first;
    }

    public override float GetMax()
    {
        return third;
    }

}
