using TMPro;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public Transform Bandit;

    void Update()
    {
        transform.position = Bandit.position;
        transform.Translate(Vector2.up * 1.55f);
    }
}
