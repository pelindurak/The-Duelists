using System;

public class TrapezoidalMF : BaseMF
{

    public float first { get; set; } = 0;
    public float second { get; set; } = 0;
    public float third { get; set; } = 0;
    public float fourth { get; set; } = 0;

    /// <param name="name">The name for the membership function.</param>
    /// <param name="first">Increasing point</param>
    /// <param name="second">Left point where membership = 1</param>
    /// <param name="third">Left point where membership = 1</param>
    /// <param name="fourth">Descend point</param>
    public TrapezoidalMF(string name, float first, float second, float third, float fourth) : base(name)
    {
        this.first = first;
        this.second = second;
        this.third = third;
        this.fourth = fourth;
    }

    public override float GetMembership(float value)
    {
        if (value >= third && value < fourth) return (fourth - value) / (fourth - third);
        else if (value >= second && value < third) return 1;
        else if (value >= first && value < second) return (value - first) / (second - first);
        else return 0;
    }

    public override float GetMin()
    {
        return first;
    }

    public override float GetMax()
    {
        return fourth;
    }

}
