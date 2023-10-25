using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GolfBall : MonoBehaviour
{
    public float rotationIncrement = 100f;
    public static float maxPuttPower = .5f;
    public float puttPowerIncrementSpeed = .5f;
    private float puttPower = 0.0f;
    public int puttCount = 0;
    private bool ballMoving = false;
    public int totalPutts = 0;
    public int putts = 0;
    public bool isTurn = false;
    public bool finishedHole = false;
    private Rigidbody rb;
    public TextMeshProUGUI puttPowerText;
    public TextMeshProUGUI puttText;
    public GameObject turnManager;
    public GameObject puttPowerBar;
    private static Image puttPowerBarImg;
    public GameObject directionArrows;
    private bool reachedHole = false;
    private bool endCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        directionArrows.SetActive(false);
        puttPowerBarImg = puttPowerBar.transform.GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        SetPuttPowerBarValue(puttPower);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurn)
        {
            float rotationAmount = 0;

            if (rb.velocity.magnitude > 0.1)
            {
                ballMoving = true;
            }
            else
            {
                ballMoving = false;
            }

            if (!ballMoving && puttCount == 1)
            {
                directionArrows.SetActive(true);
                // rotate left
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        rotationAmount = -rotationIncrement / 8;
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
                        rotationAmount = rotationIncrement / 8;
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
                    SetPuttPowerBarValue(puttPower);
                }

                // Check for space bar release to perform the putt
                if (Input.GetKeyUp(KeyCode.Space) && puttCount > 0)
                {
                    Vector3 prevPosition = transform.position;
                    GetComponent<Rigidbody>().AddForce(transform.forward * puttPower, ForceMode.Impulse);
                    puttCount -= 1;
                    totalPutts++;
                    putts++;
                    puttText.text = "Putts: " + putts;
                    puttPower = 0.0f;
                    SetPuttPowerBarValue(puttPower);
                }

                puttPowerText.text = "Putt Power: " + Mathf.Floor(puttPower * 100);

            } else if (!ballMoving && puttCount == 0 && !reachedHole && !endCalled) {
                StartCoroutine(EndTurn());

            } 
            else if (!ballMoving && puttCount == 0 && reachedHole && !endCalled) {
                StartCoroutine(ReachedHole());

            } else {

                directionArrows.SetActive(false);
            }
        }
    }

    public static void SetPuttPowerBarValue(float value)
    {
        float mappedValue = value / maxPuttPower; // Map the value to the [0,1] range
        puttPowerBarImg.fillAmount = mappedValue;
        if (puttPowerBarImg.fillAmount < 0.3f)
        {
            SetPuttPowerBarColor(Color.green);
        }
        else if (puttPowerBarImg.fillAmount < 0.7f)
        {
            SetPuttPowerBarColor(Color.yellow);
        }
        else
        {
            SetPuttPowerBarColor(Color.red);
        }
    }

    public static void SetPuttPowerBarColor(Color c)
    {
        puttPowerBarImg.color = c;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Hole"))
        {
            reachedHole = true;
        } else if (col.gameObject.CompareTag("Kill Box"))
        {
            Restart();
        }
    }

    IEnumerator ReachedHole()
    {
        yield return new WaitForSeconds(0.1f);
        if (!ballMoving && !endCalled)
        {
            endCalled = true;
            yield return new WaitForSeconds(1.5f);
            putts = 0;
            isTurn = false;
            finishedHole = true;
            gameObject.SetActive(false);
            turnManager.GetComponent<TurnManager>().nextTurn();
            reachedHole = false;
            endCalled = false;
        }
    }

    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(0.1f);
        if (!ballMoving && !endCalled)
        {
            endCalled = true;
            directionArrows.SetActive(false);
            isTurn = false;
            yield return new WaitForSeconds(1.5f);
            turnManager.GetComponent<TurnManager>().nextTurn();
            endCalled = false;
        }
    }

    void Restart()
    {
        rb.velocity = Vector3.zero;
        transform.position = turnManager.GetComponent<TurnManager>().holeLocations[turnManager.GetComponent<TurnManager>().hole];
    }
}


