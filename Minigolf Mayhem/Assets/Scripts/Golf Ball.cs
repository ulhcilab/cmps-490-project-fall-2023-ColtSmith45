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
    private bool ballMoving = false;
    public int totalPutts = 0;
    public int putts = 0;
    public bool isTurn = false;
    private Rigidbody rb;
    public TextMeshProUGUI puttPowerText;
    public TextMeshProUGUI puttText;
    public TextMeshProUGUI centerText;
    public GameObject turnManager;
    public GameObject puttPowerBar;
    private static Image puttPowerBarImg;
    public GameObject directionArrows;
    public bool reachedHole = false;
    public bool endCalled = false;
    public AudioSource audioSource;
    public AudioClip puttSoundEffect;
    public AudioClip puttPowerSoundEffect;
    public AudioClip holeSoundEffect;
    public AudioClip successSoundEffect;
    public AudioClip failureSoundEffect;
    private bool audioPlaying = false;
    private bool delay = false;
    public Image centerTxtBack;

    // Start is called before the first frame update
    void Start()
    {
        centerTxtBack.enabled = false;
        centerText.text = string.Empty;
        audioSource = GetComponent<AudioSource>();
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

            if (rb.velocity.magnitude > 0.0001)
            {
                ballMoving = true;
            }
            else
            {
                ballMoving = false;
            }

            //Ball is not moving and hasn't reached max putts
            if (!ballMoving)
            {
                //
                if (putts < TurnManager.maxPutts)
                {
                    if (!reachedHole) directionArrows.SetActive(true);
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
                    if (Input.GetKey(KeyCode.Space))
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

                        if (!audioPlaying)
                        {
                            audioPlaying = true;
                            audioSource.volume = 0.5f;
                            audioSource.pitch = (1 + puttPower) * audioSource.pitch;
                            audioSource.PlayOneShot(puttPowerSoundEffect, 1);
                            StartCoroutine(WaitForAudioFinish(puttPowerSoundEffect.length));
                        }
                    }

                    // Check for space bar release to perform the putt
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        audioSource.volume = 1;
                        audioSource.pitch = 1;
                        audioSource.PlayOneShot(puttSoundEffect, 0.25f);
                        Vector3 prevPosition = transform.position;
                        GetComponent<Rigidbody>().AddForce(transform.forward * puttPower, ForceMode.Impulse);
                        totalPutts++;
                        putts++;
                        puttText.text = "Putts: " + putts;
                        puttPower = 0.0f;
                        SetPuttPowerBarValue(puttPower);
                        StartCoroutine(DelayForTime(1));
                    }

                    puttPowerText.text = "Putt Power: " + Mathf.Floor(puttPower * 200);

                //Ball has reached max putts, has stopped moving, & hasn't reached the hole
                }
                else if (!reachedHole && !ballMoving && !delay)
                {
                    directionArrows.SetActive(false);
                    StartCoroutine(ReachedMaxPutts());
                }

            //Ball moving and still has putts left 
            } else
            {
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
        if(col.gameObject.CompareTag("Hole Drop"))
        {
            audioSource.PlayOneShot(successSoundEffect, 0.25f);
            audioSource.PlayOneShot(holeSoundEffect, 0.25f);
        } else if (col.gameObject.CompareTag("Hole"))
        {
            reachedHole = true; 
            StartCoroutine(ReachedHole());
        }
        else if (col.gameObject.CompareTag("Kill Box"))
        {
            Restart();
        } 
    }

    IEnumerator ReachedHole()
    {
        isTurn = false;
        centerTxtBack.enabled = true;
        if (putts == 1)
        {
            centerText.text = "Hole in One!";
        } else
        {
            centerText.text = "Putts: " + putts;
        }
        yield return new WaitForSeconds(3);
        centerTxtBack.enabled = false;
        centerText.text = "";
        putts = 0;
        gameObject.SetActive(false);
        TurnManager.playersFinished++;
        turnManager.GetComponent<TurnManager>().NextTurn();
    }

    IEnumerator ReachedMaxPutts()
    {
        yield return new WaitForSeconds(0.1f);
        if (!endCalled)
        {
            endCalled = true;
            isTurn = false;
            audioSource.PlayOneShot(failureSoundEffect, 0.25f);
            totalPutts += TurnManager.maxPuttPenalty;
            puttText.text = "Putts: " + (putts + TurnManager.maxPuttPenalty);
            centerTxtBack.enabled = true;
            centerText.text = "Max Putts Reached     Putts: " + (putts + TurnManager.maxPuttPenalty);
            yield return new WaitForSeconds(3);
            centerTxtBack.enabled = false;
            centerText.text = "";
            putts = 0;
            gameObject.SetActive(false);
            TurnManager.playersFinished++;
            turnManager.GetComponent<TurnManager>().NextTurn();
        }
    }

    void Restart()
    {
        rb.velocity = Vector3.zero;
        transform.position = turnManager.GetComponent<TurnManager>().holeLocations[turnManager.GetComponent<TurnManager>().hole];
    }

    IEnumerator WaitForAudioFinish(float audioLength)
    {
        yield return new WaitForSeconds(audioLength);
        audioPlaying = false;
    }

    IEnumerator DelayForTime(float delayTime)
    {
        delay = true;
        yield return new WaitForSeconds(delayTime);
        delay = false;
    }
}


