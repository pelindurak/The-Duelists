using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnFsmClick()
    {
        // In build settings:
        // PlayerVSFsmScene = 1
        SceneManager.LoadScene(1);
    }

    public void OnFuzzyClick()
    {
        // In build settings:
        // PlayerVSFuzzyScene = 2
        SceneManager.LoadScene(2);
    }

}
