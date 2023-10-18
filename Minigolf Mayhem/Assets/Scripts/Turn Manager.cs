using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public int maxPossiblePlayers = 4;
    public int playerCount = 4;
    public int holeStart = 1;
    private int turn = 0;
    private int hole;
    public Tuple<Camera, GameObject>[] cameraPlayerTuples;
    public TextMeshProUGUI playerName;
    public Vector3[] holeLocations;

    void Start()
    {
        hole = holeStart - 1;
        DisableNonPlayers();
        FillCameraPlayerTuples();
        for (int i = 1; i <= playerCount; i++)
        {
            cameraPlayerTuples[i - 1].Item2.SetActive(false);
            cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.position = holeLocations[hole];
        }
        nextTurn();

    }

    void Update()
    {
        // Your update logic here
    }

    public void FillCameraPlayerTuples()
    {
        cameraPlayerTuples = new Tuple<Camera, GameObject>[playerCount];

        for (int i = 1; i <= playerCount; i++)
        {
            Camera cam = GameObject.Find("Cam " + i).GetComponent<Camera>();
            GameObject player = GameObject.Find("Golf Ball " + i.ToString());

            if (cam != null && player != null)
            {
                cameraPlayerTuples[i - 1] = new Tuple<Camera, GameObject>(cam, player);
            }
            else
            {
                Debug.LogError("Camera or Player not found for index " + i);
            }
        }
    }

    public static void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1); 
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public void nextTurn()
    {
        Debug.Log("Next Turn Called");
        if (turn + 1 > playerCount)
        {
            turn = 1;
        } 
        else
        {
            turn += 1;
        }
        Debug.Log(turn);


        int playersFinished = 0;
        for (int i = 1; i <= playerCount; i++)
        {
            if (cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().finishedHole) playersFinished++;
        }
        if (playersFinished == playerCount)
        {
            nextHole();
            return;
        }


        for (int i = 1; i <= playerCount; i++)
        {
            if (i == turn)
            {
                if (cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().finishedHole)
                {
                    nextTurn();
                    return;
                }
                cameraPlayerTuples[i - 1].Item2.SetActive(true);
                cameraPlayerTuples[i - 1].Item1.enabled = true;
                playerName.text = cameraPlayerTuples[i - 1].Item2.name;
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().isTurn = true;
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().turnTime = 0;
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().puttCount = 1;
            } 
            else
            {
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().isTurn = false;
                cameraPlayerTuples[i - 1].Item1.enabled = false;
            }
        }

        Debug.Log("Reached end of Turn()");
    }

    public void nextHole()
    {
        Debug.Log("Next Hole Called");
        hole++; 
        for (int i = 1; i <= playerCount; i++)
        {
            cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.position = holeLocations[hole];
            cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.eulerAngles = new Vector3(0f, -90f, 0f);
            cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().finishedHole = false;
        }
        turn = 0;
        nextTurn();

    }

    public void DisableNonPlayers()
    {
        Debug.Log("DisableNonPlayers Called");
        for (int i = playerCount + 1; i <= maxPossiblePlayers ; i++)
        {
            Camera cam = GameObject.Find("Cam " + i).GetComponent<Camera>();
            GameObject player = GameObject.Find("Golf Ball " + i.ToString());

            if (cam != null && player != null)
            {
                cam.enabled = false;
                player.SetActive(false);
                Debug.Log("Disabled " + player.name);
            }
            else
            {
                Debug.LogError("Camera or Player not found for index " + i);
            }
        }
    }
}
