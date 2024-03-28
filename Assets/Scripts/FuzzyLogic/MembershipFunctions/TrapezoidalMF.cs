using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapezoidalMF
{

    public string name { get; set; }
    public float first { get; set; } = 0;
    public float second { get; set; } = 0;
    public float third { get; set; } = 0;
    public float fourth { get; set; } = 0;

    /// <param name="name">The name for the membership function.</param>
    /// <param name="first">Increasing point</param>
    /// <param name="second">Left point where membership = 1</param>
    /// <param name="third">Left point where membership = 1</param>
    /// <param name="fourth">Descend point</param>
    public TrapezoidalMF(String name, float first, float second, float third, float fourth)
    {
        this.name = name;
        this.first = first;
        this.second = second;
        this.third = third;
        this.fourth = fourth;
    }

    public float Fuzzify(float crispValue)
    {
        if (crispValue >= third && crispValue < fourth) return (fourth - crispValue) / (fourth - third);
        else if (crispValue >= second && crispValue < third) return 1;
        else if (crispValue >= first && crispValue < second) return (crispValue - first) / (second - first);
        else return 0;
    }

    public float GetMin()
    {
        return first;
    }

    public float GetMax()
    {
        return fourth;
    }

}
