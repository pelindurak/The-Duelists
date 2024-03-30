
using System;
using System.Collections.Generic;

public class AggressionRule 
{

    List<MemPair> list1;
    List<MemPair> list2;

    /// <param name="list1"> Enemy Health Fuzzy Set </param>
    /// <param name="list2"> Player Health Fuzzy Set </param>
    public AggressionRule(List<MemPair> list1, List<MemPair> list2)
    {
        this.list1 = list1;
        this.list2 = list2;
    }

    public List<MemPair> EvaluateRule()
    {
        return new List<MemPair>
        {
            LowAggro(),
            MidAggro(),
            HighAggro()
        };
    }


    // If health IS low & player health IS NOT low
    // OR
    // If health IS high & player health IS high
    private MemPair LowAggro()
    {
        float health = list1[0].value;
        float playerHealth = list2[0].value;
        float health2 = list1[2].value;
        float playerHealth2 = list2[2].value;
        float value = Math.Max(Math.Min(health, (1 - playerHealth)), Math.Min(health2, playerHealth2));
        return new MemPair("LOW", value);
    }


    // If health IS medium & player health IS NOT low
    // OR
    // If health IS low & player health IS low
    private MemPair MidAggro()
    {
        float health = list1[1].value;
        float playerHealth = list2[0].value;
        float health2 = list1[0].value;
        float playerHealth2 = list2[0].value;
        float value = Math.Max(Math.Min(health, (1 - playerHealth)), Math.Min(health2, playerHealth2));
        return new MemPair("MEDIUM", value);
    }


    // If health IS high & player health IS NOT high
    // OR
    // If health IS medium & player health IS low
    private MemPair HighAggro()
    {
        float health = list1[2].value;
        float playerHealth = list2[2].value;
        float health2 = list1[1].value;
        float playerHealth2 = list2[0].value;
        float value = Math.Max(Math.Min(health, (1 - playerHealth)), Math.Min(health2, playerHealth2));
        return new MemPair("HIGH", value);
    }

}
