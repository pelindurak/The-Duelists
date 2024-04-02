using System.Collections.Generic;
using UnityEngine;

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

    private float crispAggro = 0f;


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
        if (banditScript.IsFuzzyLogicActive)
        {
            UpdateAggressionValue();
        }
    }

    public float GetCrispAggro()
    {
        return crispAggro;
    }

    private void UpdateAggressionValue()
    {
        BanditHealth = banditScript.BanditHealth;
        PlayerHealth = banditScript.PlayerHealth;

        playerList = healthSet.FuzzyValueList(PlayerHealth);
        enemyList = healthSet.FuzzyValueList(BanditHealth);
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
