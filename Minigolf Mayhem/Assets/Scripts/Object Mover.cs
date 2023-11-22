using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private bool movingPos = true;

    private void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(Delay());
    }

    private void Update()
    {

        if (delayFinished)
        {
            if (moveHorizontally)
            {
                HorizontalMovement();
            }
            else if (moveVertically)
            {
                VerticalMovement();
            }
        }
    }

    public void HorizontalMovement()
    {
        if (!paused)
        {
            if (movingPos)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                if (Vector3.Distance(originalPosition, transform.position) >= moveDistance)
                {
                    if (!paused) StartCoroutine(Pause());
                    movingPos = false;
                }
            }
            else
            {
                transform.Translate(Vector3.back * Time.deltaTime * backSpeed);
                if (transform.position.z - originalPosition.z <= 0)
                {
                    transform.position = originalPosition;
                    if (!paused) StartCoroutine(Pause());
                    movingPos = true;
                }
            }

        }

    }

    public void VerticalMovement()
    {
        if (!paused)
        {
            if (movingPos)
            {
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
                if (Vector3.Distance(originalPosition, transform.position) >= moveDistance)
                {
                    if (!paused) StartCoroutine(Pause());
                    movingPos = false;
                }
            }
            else
            {
                transform.Translate(Vector3.down * Time.deltaTime * backSpeed);
                if (transform.position.y - originalPosition.y <= 0)
                {
                    transform.position = originalPosition; 
                    if (!paused) StartCoroutine(Pause());
                    movingPos = true;
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



