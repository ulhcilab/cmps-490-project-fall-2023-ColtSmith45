using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        gameOverText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Golf Ball"))
        {
            Debug.Log("Ball Reached Hole");
            gameOverText.enabled = true;
        }
        
    }
}
