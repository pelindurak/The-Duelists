public abstract class BaseMF
{
    public string Name { get; set; }

    public BaseMF(string name) 
    {
        Name = name;
    }

    public abstract float GetMin();

    public abstract float GetMax();

    public abstract float GetMembership(float value);

    public abstract float CalculateCentroid(float value);
    public abstract float CalculateArea(float value);

}
