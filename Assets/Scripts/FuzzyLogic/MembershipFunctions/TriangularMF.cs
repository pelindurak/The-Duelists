using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangularMF
{
    public string name { get; set; }
    public float first { get; set; } = 0;
    public float second { get; set; } = 0;
    public float third { get; set; } = 0;

    /// <param name="name">The name for the membership function.</param>
    /// <param name="first">Increasing point</param>
    /// <param name="second">Point where membership = 1</param>
    /// <param name="third">Descend point</param>
    public TriangularMF(string name, float first, float second, float third)
    {
        this.name = name;
        this.first = first;
        this.second = second;
        this.third = third;
    }

    public float Fuzzify(float crispValue)
    {
        if (crispValue >= second && crispValue < third) return (third - crispValue) / (third - second);
        else if (crispValue > first && crispValue < second) return (crispValue - first) / (second - first);
        else return 0;
    }

    public float GetMin()
    {
        return first;
    }

    public float GetMax()
    {
        return third;
    }

}
