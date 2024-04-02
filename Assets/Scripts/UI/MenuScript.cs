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

    public void OnVersusClick()
    {
        // In build settings:
        // FsmVSFuzzyScene = 3
        SceneManager.LoadScene(3);
    }

    public void OnRestartFuzzyClick()
    {
        OnFuzzyClick();
    }

    public void OnRestartFsmClick()
    {
        OnFsmClick();
    }

    public void GoBackToMainMenu()
    {
        // In build settings:
        // MainScene = 0
        SceneManager.LoadScene(0);
    }

}
