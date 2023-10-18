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
    public int holeEnd = 1;
    private int turn = 0;
    public int hole;
    public Tuple<Camera, GameObject>[] cameraPlayerTuples;
    public TextMeshProUGUI playerName;
    public Vector3[] holeLocations;
    public TextMeshProUGUI firstPlaceNameTxt;
    public TextMeshProUGUI firstPlacePuttTxt;
    public TextMeshProUGUI secondPlaceNameTxt;
    public TextMeshProUGUI secondPlacePuttTxt;
    public TextMeshProUGUI thirdPlaceNameTxt;
    public TextMeshProUGUI thirdPlacePuttTxt;
    public TextMeshProUGUI fourthPlaceNameTxt;
    public TextMeshProUGUI fourthPlacePuttTxt;
    public Canvas playerScreen;
    public Canvas scoreboard;
    public Camera endScreenCam;

    void Start()
    {
        endScreenCam.enabled = false;
        playerScreen.enabled = true;
        scoreboard.enabled = false;
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
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().puttCount = 1;
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().puttText.text = "Putts: " + cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().putts;

            } 
            else
            {
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().isTurn = false;
                cameraPlayerTuples[i - 1].Item1.enabled = false;
            }
        }
    }

    public void nextHole()
    {
        hole++; 
        if (hole > holeEnd - 1)
        {
            EndGame();
        } else
        {
            for (int i = 1; i <= playerCount; i++)
            {
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.position = holeLocations[hole];
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.eulerAngles = new Vector3(0f, -90f, 0f);
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().finishedHole = false;
            }
            turn = 0;
            nextTurn();
        }
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

    void EndGame()
    {
        endScreenCam.enabled = true;
        firstPlaceNameTxt.enabled = false;
        firstPlacePuttTxt.enabled = false;
        secondPlaceNameTxt.enabled = false;
        secondPlacePuttTxt.enabled = false;
        thirdPlaceNameTxt.enabled = false;
        thirdPlacePuttTxt.enabled = false;
        fourthPlaceNameTxt.enabled = false;
        fourthPlacePuttTxt.enabled = false;

        int[] puttScores = new int[playerCount];
        for (int i = 1; i <= playerCount; i++)
        {
            puttScores[i - 1] = cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().totalPutts;
        }

        Array.Sort(puttScores);




        for (int i = 0; i < playerCount; i++)
        {
            Debug.Log(cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString());
            Debug.Log(puttScores[i].ToString());
         
            if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[i] && i == 0)
            {
                firstPlaceNameTxt.text = cameraPlayerTuples[0].Item2.name;
                firstPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();
                firstPlaceNameTxt.enabled = true;
                firstPlacePuttTxt.enabled = true;

            } else if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[i] && i == 1)
            {
                secondPlaceNameTxt.text = cameraPlayerTuples[i].Item2.name;
                secondPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();
                secondPlaceNameTxt.enabled = true;
                secondPlacePuttTxt.enabled = true;

            } else if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[i] && i == 2)
            {
                thirdPlaceNameTxt.text = cameraPlayerTuples[i].Item2.name;
                thirdPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();
                thirdPlaceNameTxt.enabled = true;
                thirdPlacePuttTxt.enabled = true;

            } else if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[i] && i == 3)
            {
                fourthPlaceNameTxt.text = cameraPlayerTuples[i].Item2.name;
                fourthPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();
                fourthPlaceNameTxt.enabled = true;
                fourthPlacePuttTxt.enabled = true;

            }

        }

        playerScreen.enabled = false;
        scoreboard.enabled = true;
    }
}
