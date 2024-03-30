using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyMain : MonoBehaviour
{

    FuzzySet healthSet;
    FuzzySet enemyHealthSet;
    FuzzySet distanceSet;

    public float Health;

    List<MemPair> playerList = new List<MemPair>();
    List<MemPair> enemyList = new List<MemPair>();
    List<MemPair> aggressionList = new List<MemPair>();

    // Start is called before the first frame update
    void Start()
    {
        healthSet = InitHealthSet();
        enemyHealthSet = InitHealthSet();

        playerList = healthSet.FuzzyValueList(25f);
        enemyList = healthSet.FuzzyValueList(50f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EvaluateAggression()
    {
        AggressionRule aggression = new AggressionRule(enemyList, playerList);
        aggressionList = aggression.EvaluateRule();
    }

    FuzzySet InitHealthSet()
    {
        FuzzySet fuzzySet = new FuzzySet();
        fuzzySet.AddDescendingLinearMF("LOW", 0f, 40f);
        fuzzySet.AddTriangularMF("MEDIUM", 20f, 50f, 80f);
        fuzzySet.AddAscendingLinearMF("HIGH", 60f, 100f);
        return fuzzySet;
    }
}
