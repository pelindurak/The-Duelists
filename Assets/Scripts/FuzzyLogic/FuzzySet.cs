using System.Collections.Generic;

public class FuzzySet
{

    public List<BaseMF> functionList {  get; set; }

    public FuzzySet() 
    {
        functionList = new List<BaseMF>();
    }

    public float Defuzzify(List<MemPair> valueList)
    {
        float sum = 0f, weights = 0f;
        for (int i = 0; i < functionList.Count; i++)
        {
            BaseMF mf = functionList[i];
            float area = mf.CalculateArea(valueList[i].value);
            float centroid =  mf.CalculateCentroid(valueList[i].value);

            sum += area * centroid;
            weights += area;
        }
        if (weights == 0f) return 0f;

        return sum / weights;
    }

    public List<MemPair> FuzzyValueList(float value)
    {
        List<MemPair> membershipList = new List<MemPair>();
        foreach (BaseMF m in functionList)
        {
            MemPair mp = new MemPair(m.Name, m.GetMembership(value));
            membershipList.Add(mp);
        }
        return membershipList;
    }

    public void AddTriangularMF(string name, float first, float second, float third)
    {
        BaseMF mf = new TriangularMF(name, first, second, third);
        functionList.Add(mf);
    }

    public void AddTrapezoidalMF(string name, float first, float second, float third, float fourth)
    {
        BaseMF mf = new TrapezoidalMF(name, first, second, third, fourth);
        functionList.Add(mf);
    }

    public void AddAscendingLinearMF(string name, float first, float second)
    {
        BaseMF mf = new AscendingLinearMF(name, first, second);
        functionList.Add(mf);
    }

    public void AddDescendingLinearMF(string name, float first, float second)
    {
        BaseMF mf = new DescendingLinearMF(name, first, second);
        functionList.Add(mf);
    }

    public string GetListAsString(List<MemPair> membershipList)
    {
        string str = "";
        foreach (MemPair m in membershipList)
        {
            str += m.ToString() + " / ";
        }
        return str;
    }

}
