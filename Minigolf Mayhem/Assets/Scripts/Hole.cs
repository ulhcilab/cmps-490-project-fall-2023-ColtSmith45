using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Terrain terrain; // Reference to your Terrain object
    public Vector2 holeSize = new Vector2(0.15f, 0.15f); // Size of the hole
    public float holeDepth = 2.0f; // Depth of the hole


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {

    }
}
