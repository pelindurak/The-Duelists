using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuzzyMain : MonoBehaviour
{

    private FuzzySet healthSet;
    private FuzzySet enemyHealthSet;
    private FuzzySet aggroSet;

    private List<MemPair> playerList = new List<MemPair>();
    private List<MemPair> enemyList = new List<MemPair>();
    private List<MemPair> aggressionList = new List<MemPair>();


    private Bandit banditScript;
    private float BanditHealth = 0f, PlayerHealth = 0f;
    private FuzzySet distanceSet;


    public float crispAggro = 0f;


    void Start()
    {
        banditScript = GetComponent<Bandit>();
        BanditHealth = banditScript.BanditHealth;
        PlayerHealth = banditScript.PlayerHealth;

        healthSet = InitHealthSet();
        enemyHealthSet = InitHealthSet();
        aggroSet = InitAggroSet();
    }

    void Update()
    {
        UpdateAggressionValue();
    }

    private void UpdateAggressionValue()
    {
        BanditHealth = banditScript.BanditHealth;

        playerList = healthSet.FuzzyValueList(PlayerHealth * 100);
        enemyList = healthSet.FuzzyValueList(BanditHealth * 100);
        aggressionList = EvaluateAggression();

        crispAggro = aggroSet.Defuzzify(aggressionList);

        Debug.Log($"Aggression: {GetListAsString(aggressionList)} crisp value: {crispAggro}");
    }

    private List<MemPair> EvaluateAggression()
    {
        AggressionRule aggression = new AggressionRule(enemyList, playerList);
        return aggression.EvaluateRule();
    }

    private FuzzySet InitHealthSet()
    {
        FuzzySet fuzzySet = new FuzzySet();
        fuzzySet.AddDescendingLinearMF("LOW", 0f, 40f);
        fuzzySet.AddTriangularMF("MEDIUM", 20f, 50f, 80f);
        fuzzySet.AddAscendingLinearMF("HIGH", 60f, 100f);
        return fuzzySet;
    }

    private FuzzySet InitAggroSet()
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
