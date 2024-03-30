using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyMain : MonoBehaviour
{

    FuzzySet healthSet;
    FuzzySet enemyHealthSet;
    FuzzySet aggroSet;

    FuzzySet distanceSet;

    public float Health;

    List<MemPair> playerList = new List<MemPair>();
    List<MemPair> enemyList = new List<MemPair>();
    List<MemPair> aggressionList = new List<MemPair>();

    float crispAggro;

    // Start is called before the first frame update
    void Start()
    {
        healthSet = InitHealthSet();
        enemyHealthSet = InitHealthSet();
        aggroSet = InitAggroSet();

        playerList = healthSet.FuzzyValueList(5f);
        enemyList = healthSet.FuzzyValueList(65f);
        aggressionList = EvaluateAggression();

        Debug.Log(GetListAsString(aggressionList));
        

        crispAggro = aggroSet.Defuzzify(aggressionList);

        Debug.Log(crispAggro);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<MemPair> EvaluateAggression()
    {
        AggressionRule aggression = new AggressionRule(enemyList, playerList);
        return aggression.EvaluateRule();
    }

    FuzzySet InitHealthSet()
    {
        FuzzySet fuzzySet = new FuzzySet();
        fuzzySet.AddDescendingLinearMF("LOW", 0f, 40f);
        fuzzySet.AddTriangularMF("MEDIUM", 20f, 50f, 80f);
        fuzzySet.AddAscendingLinearMF("HIGH", 60f, 100f);
        return fuzzySet;
    }

    FuzzySet InitAggroSet()
    {
        FuzzySet fuzzySet = new FuzzySet();
        fuzzySet.AddDescendingLinearMF("LOW", 0f, 40f);
        fuzzySet.AddTriangularMF("MEDIUM", 20f, 50f, 80f);
        fuzzySet.AddAscendingLinearMF("HIGH", 60f, 100f);
        return fuzzySet;
    }

    private string GetListAsString(List<MemPair> membershipList)
    {
        string str = "";
        foreach (MemPair m in membershipList)
        {
            str += m.ToString() + " / ";
        }
        return str;
    }
}
