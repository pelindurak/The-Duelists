using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzySet
{

    public List<BaseMF> functionList {  get; set; }

    public FuzzySet() 
    {
        functionList = new List<BaseMF>();
    }

    public void Defuzzify()
    {
        // TODO Defuzzify
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
