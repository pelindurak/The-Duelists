using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmMain : MonoBehaviour
{

    private Bandit banditScript;
    private float BanditHealth = 0f, PlayerHealth = 0f;

    void Start()
    {
        banditScript = GetComponent<Bandit>();
        BanditHealth = banditScript.BanditHealth;
        PlayerHealth = banditScript.PlayerHealth;
    }

    void Update()
    {
        BanditHealth = banditScript.BanditHealth;
        PlayerHealth = banditScript.PlayerHealth;


    }
}
