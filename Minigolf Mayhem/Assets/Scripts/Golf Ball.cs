using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float maxPuttPower = 2.0f;
    public float puttPowerIncrementSpeed = 5.0f;
    private float puttPower = 0.0f;
    private new GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        if (camera == null )
        {
            Debug.LogError("Camera not found in scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAmount = 0f;

        // Check for input to rotate left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotationAmount = -rotationSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) rotationAmount = -rotationSpeed/4;
        }
        // Check for input to rotate right
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotationAmount = rotationSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) rotationAmount = rotationSpeed / 4;
        }

        // Apply rotation to the parent object
        transform.Rotate(Vector3.up * rotationAmount * Time.deltaTime);
        if (camera != null)
        {
            camera.transform.Rotate(Vector3.up * rotationAmount * Time.deltaTime);
            camera.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z - 1.2f);
        }

        // Check for space bar input to control putt power
        if (Input.GetKey(KeyCode.Space))
        {
            if (puttPower < maxPuttPower) puttPower += puttPowerIncrementSpeed * Time.deltaTime;
        }

        // Check for space bar release to perform the putt
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * puttPower, ForceMode.Impulse);
            puttPower = 0.0f;
        }

    }


}
