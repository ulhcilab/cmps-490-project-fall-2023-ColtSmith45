using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float moveDistance = 5f;
    public float moveSpeed = 2f;
    public float backSpeed = 2f;
    public float pauseTime = 5f;
    public float delay = 0;
    private bool delayFinished = false;
    public bool moveHorizontally = true;
    public bool moveVertically = false;
    private bool paused = false;

    private Vector3 originalPosition;
    private bool movingForward = true;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        StartCoroutine(Delay());

        if (delayFinished)
        {
            if (moveHorizontally)
            {
                MoveHorizontally();
            }
            else if (moveVertically)
            {
                MoveVertically();
            }

        }
    }

    private void MoveHorizontally()
    {
        if (!paused)
        {
            if (movingForward)
            {
                transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
                if (Vector3.Distance(originalPosition, transform.position) >= moveDistance)
                {
                    StartCoroutine(Pause());
                    movingForward = false;
                }
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * backSpeed);
                if (Vector3.Distance(originalPosition, transform.position) <= 0.01f)
                {
                    StartCoroutine(Pause());
                    movingForward = true;
                }
            }

        }
    }

    private void MoveVertically()
    {
        if (!paused)
        {
            if (movingForward)
            {
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
                if (Vector3.Distance(originalPosition, transform.position) >= moveDistance)
                {
                    if (!paused) StartCoroutine(Pause());
                    movingForward = false;
                }
            }
            else
            {
                transform.Translate(Vector3.down * Time.deltaTime * backSpeed);
                if (Vector3.Distance(originalPosition, transform.position) <= 0.01f)
                {
                    if (!paused) StartCoroutine(Pause());
                    movingForward = true;
                }
            }

        }
    }

    IEnumerator Pause()
    {
        paused = true;
        yield return new WaitForSeconds(pauseTime);
        paused = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        delayFinished = true;
    }
}



