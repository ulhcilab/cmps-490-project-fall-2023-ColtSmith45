using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    Transform pivot;
    public float rotationSpeed = 50f;
    public bool rotateHorizontally = false;
    public bool rotateVertically = false;
    public bool rotateRight = false;
    public bool rotateLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        pivot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateVertically)
        {
            if (rotateRight) transform.RotateAround(pivot.position, Vector3.right, rotationSpeed * Time.deltaTime);

            if (rotateLeft) transform.RotateAround(pivot.position, Vector3.left, rotationSpeed * Time.deltaTime);
        }

        if (rotateHorizontally)
        {
            if (rotateRight) transform.RotateAround(pivot.position, Vector3.up, rotationSpeed * Time.deltaTime);

            if (rotateLeft) transform.RotateAround(pivot.position, Vector3.down, rotationSpeed * Time.deltaTime);
        }
    }
}
