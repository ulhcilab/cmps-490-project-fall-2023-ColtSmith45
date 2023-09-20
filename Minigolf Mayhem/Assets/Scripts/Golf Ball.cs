using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    public float rotationIncrement = 100f;
    public float maxPuttPower = 1;
    public float puttPowerIncrementSpeed = 1;
    private float puttPower = 0.0f;
    public int puttCount = 0;
    private bool ballMoving = false;
    private int totalPutts = 0;
    private float totalMTraveled = 0;
    private int totalResets = 0;
    public bool isTurn = false;
    public float turnTime = 0;
    private Vector3 prevPosition;
    private Vector3 newPosition;
    private Rigidbody rb;
    public TextMeshProUGUI puttPowerText;
    public GameObject turnManager;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = GameObject.Find("Turn Manager");
        rb = GetComponent<Rigidbody>();
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurn)
        {
            turnTime += Time.deltaTime;
            float rotationAmount = 0;

            if (rb.velocity.magnitude > 0.1)
            {
                ballMoving = true;
            }
            else
            {
                ballMoving = false;
            }

            if (!ballMoving)
            {
                newPosition = transform.position;
                if (newPosition != prevPosition)
                {
                    totalMTraveled += Vector3.Distance(newPosition, prevPosition);
                    prevPosition = newPosition;
                }

                // rotate left
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        rotationAmount = -rotationIncrement / 4;
                    }
                    else
                    {
                        rotationAmount = -rotationIncrement;
                    }
                    transform.Rotate(0, rotationAmount * Time.deltaTime, 0);
                }
                // rotate right
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        rotationAmount = rotationIncrement / 4;
                    }
                    else
                    {
                        rotationAmount = rotationIncrement;
                    }
                    transform.Rotate(0, rotationAmount * Time.deltaTime, 0);
                }

                // Check for space bar input to control putt power
                if (Input.GetKey(KeyCode.Space) && puttCount > 0)
                {
                    if (puttPower + puttPowerIncrementSpeed * Time.deltaTime > maxPuttPower)
                    {
                        puttPower = maxPuttPower;
                    }
                    else
                    {
                        puttPower += puttPowerIncrementSpeed * Time.deltaTime;
                    }
                }

                // Check for space bar release to perform the putt
                if (Input.GetKeyUp(KeyCode.Space) && puttCount > 0)
                {
                    Vector3 prevPosition = transform.position;
                    GetComponent<Rigidbody>().AddForce(transform.forward * puttPower, ForceMode.Impulse);
                    puttCount -= 1;
                    totalPutts++;
                    puttPower = 0.0f;
                }

                puttPowerText.text = "Putt Power: " + puttPower;

                if (Input.GetKeyUp(KeyCode.Q) && turnTime > 1)
                {
                    isTurn = false;
                    turnManager.GetComponent<TurnManager>().nextTurn();
                }




                if (Input.GetKeyUp(KeyCode.L))
                {
                    Debug.Log("Total Putts: " + totalPutts);
                    Debug.Log("Total Distance Traveled: " + totalMTraveled + "m");
                }

            }
        }
    }
}


