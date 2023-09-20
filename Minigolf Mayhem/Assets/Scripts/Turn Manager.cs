using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public int playerCount = 4;
    private int turn = 0;
    public Tuple<Camera, GameObject>[] cameraPlayerTuples;
    public TextMeshProUGUI playerName;

    void Start()
    {
        FillCameraPlayerTuples();
        Shuffle(cameraPlayerTuples);
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
            int j = UnityEngine.Random.Range(0, i + 1); // Use UnityEngine.Random.Range
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public void nextTurn()
    {
        if (turn + 1 > playerCount)
        {
            turn = 1;
        } 
        else
        {
            turn += 1;
        }

        Debug.Log(turn);

        for (int i = 1; i <= playerCount; i++)
        {
            if (i == turn)
            {
                cameraPlayerTuples[i - 1].Item1.enabled = true;
                playerName.text = cameraPlayerTuples[i - 1].Item2.name;
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().isTurn = true;
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().turnTime = 0;
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().puttCount = 1;
            }
            else
            {
                cameraPlayerTuples[i - 1].Item1.enabled = false;
            }
        }
    }
}
