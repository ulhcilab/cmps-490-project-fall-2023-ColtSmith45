using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TurnManager : MonoBehaviour
{
    public int playerCount = 4;
    public int holeStart = 1;
    public int holeEnd = 1;
    public int maxPuttPenalty = 4;
    public int maxPutts = 4;
    private int turn = 0;
    public int hole;
    public Tuple<Camera, GameObject>[] cameraPlayerTuples;
    public TextMeshProUGUI playerName;
    public Vector3[] holeLocations;
    public TextMeshProUGUI holeTxt;
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
    public GameObject ball1;
    public GameObject ball2;
    public GameObject ball3;
    public GameObject ball4;
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;
    public int playersFinished = 0;
    public String[] ballHexColors = {"#FFFFFF", "#FFFFFF", "#FFFFFF", "#FFFFFF"};
    public String[] playerNames = {"Player 1", "Player 2", "Player 3", "Player 4"};

    void Start()
    {
        endScreenCam.enabled = false;
        playerScreen.enabled = true;
        scoreboard.enabled = false;
        hole = holeStart - 1;
        holeTxt.text = "Hole: " + holeStart;
        DisableNonPlayers();
        FillCameraPlayerTuples();
        for (int i = 1; i <= playerCount; i++)
        {
            cameraPlayerTuples[i - 1].Item2.SetActive(false);
            cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.position = holeLocations[hole];
        }
        NextTurn();

    }

    void Update()
    {
        // Your update logic here
    }

    public void FillCameraPlayerTuples()
    {
        cameraPlayerTuples = new Tuple<Camera, GameObject>[playerCount];
        cameraPlayerTuples[0] = new Tuple<Camera, GameObject>(cam1, ball1);
        cameraPlayerTuples[0].Item2.GetComponent<GolfBall>().ballColorHex = ballHexColors[0];

        if (playerCount == 1) return;
        cameraPlayerTuples[1] = new Tuple<Camera, GameObject>(cam2, ball2);
        cameraPlayerTuples[1].Item2.GetComponent<GolfBall>().ballColorHex = ballHexColors[1];

        if (playerCount == 2) return;
        cameraPlayerTuples[2] = new Tuple<Camera, GameObject>(cam3, ball3);
        cameraPlayerTuples[2].Item2.GetComponent<GolfBall>().ballColorHex = ballHexColors[2];

        if (playerCount == 3) return;
        cameraPlayerTuples[3] = new Tuple<Camera, GameObject>(cam4, ball4);
        cameraPlayerTuples[3].Item2.GetComponent<GolfBall>().ballColorHex = ballHexColors[3];
    }

    public void NextTurn()
    {
        int prevTurn = turn;

        if (turn + 1 > playerCount)
        {
            turn = 1;
        } 
        else
        {
            turn += 1;
        }

        if (playersFinished == playerCount)
        {
            playersFinished = 0;
            nextHole();
            return;
        }

        if (prevTurn != 0)
        {
            cameraPlayerTuples[prevTurn - 1].Item2.SetActive(false);
            cameraPlayerTuples[prevTurn - 1].Item1.enabled = false;
            cameraPlayerTuples[prevTurn - 1].Item2.GetComponent<GolfBall>().isTurn = false;
            cameraPlayerTuples[prevTurn - 1].Item2.GetComponent<GolfBall>().reachedHole = false;
            cameraPlayerTuples[prevTurn - 1].Item2.GetComponent<GolfBall>().endCalled = false;
        }

        cameraPlayerTuples[turn - 1].Item2.SetActive(true);
        cameraPlayerTuples[turn - 1].Item1.enabled = true;
        playerName.text = playerNames[turn - 1];
        cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().isTurn = true;
        cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().puttText.text = "Putts: " + cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().putts;
    }

    public void nextHole()
    {
        hole++;
        if (hole > holeEnd - 1)
        {
            EndGame();
        } else
        {
            holeTxt.text = "Hole: " + (hole + 1);
            for (int i = 1; i <= playerCount; i++)
            {
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.position = holeLocations[hole];
                cameraPlayerTuples[i - 1].Item2.GetComponent<GolfBall>().transform.eulerAngles = new Vector3(0f, -90f, 0f);
            }
            turn = 0;
            NextTurn();
        }
    }

    public void DisableNonPlayers()
    {
        for (int i = playerCount + 1; i <= 4; i++)
        {
            Camera cam = GameObject.Find("Cam " + i).GetComponent<Camera>();
            GameObject player = GameObject.Find("Golf Ball " + i.ToString());

            if (cam != null && player != null)
            {
                cam.enabled = false;
                player.SetActive(false);
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

            if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[i] && i == 0)
            {
                firstPlaceNameTxt.text = cameraPlayerTuples[i].Item2.name;
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
