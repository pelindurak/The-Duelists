
public class MemPair
{
    public string name {  get; private set; }
    public float value { get; private set; }

    public MemPair(string name, float value)
    {
        this.name = name;
        this.value = value;
    }

    public override string ToString()
    {
        return $"{name}: {value}";
    }
}
