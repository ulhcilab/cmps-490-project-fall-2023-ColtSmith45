using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for user input or modify the rotation angle as per your requirement
        float rotationAngle = Time.deltaTime * rotationSpeed;

        // Get the current rotation
        Quaternion oldRotation = transform.rotation;

        // Calculate the new rotation based on the X-axis rotation
        Quaternion newRotation = Quaternion.AngleAxis(rotationAngle, Vector3.right) * oldRotation;

        // Apply the new rotation
        transform.rotation = newRotation;
    }
}
