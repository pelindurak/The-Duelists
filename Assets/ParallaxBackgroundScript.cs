using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundScript : MonoBehaviour
{
    public Transform Player;
    public float xOffset = 0f;
    public float yOffset = 0f;
    public float zOffset = 0f;
    
    void Update()
    {
        transform.position = Player.position;
        Vector3 pos = new Vector3(xOffset, yOffset, zOffset);
        transform.Translate(pos);
    }
}
