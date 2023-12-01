using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public static int playerCount = 1;
    public static int holeStart = 1;
    public static int holeEnd = 1;
    public static int maxPuttPenalty = 2;
    public static int maxPutts = 8;
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
    public Canvas pauseScreen;
    public Canvas controlsScreen;
    public Camera endScreenCam;
    public GameObject ball1;
    public GameObject ball2;
    public GameObject ball3;
    public GameObject ball4;
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;
    public static int playersFinished = 0;
    public static String[] ballHexColors = { "#FFFFFF", "#FFFFFF", "#FFFFFF", "#FFFFFF" };
    public static String[] playerNames = { "Player 1", "Player 2", "Player 3", "Player 4" };
    public AudioClip clickSoundEffect;
    private AudioSource audioSource;
    private bool paused = false;
    private bool reset = false;

    void Start()
    {
        Time.timeScale = 1;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[1];
        endScreenCam.enabled = false;
        scoreboard.enabled = false;
        pauseScreen.enabled = false;
        controlsScreen.enabled = false;
        playerScreen.enabled = true;
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
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            paused = true;
            if (playerScreen.enabled)
            {
                audioSource.PlayOneShot(clickSoundEffect, 0.5f);
                playerScreen.enabled = false;
                cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().isTurn = false;
                pauseScreen.enabled = true;
                Time.timeScale = 0;
            } else if (controlsScreen.enabled)
            {
                audioSource.PlayOneShot(clickSoundEffect, 0.5f);
                controlsScreen.enabled = false;
                pauseScreen.enabled = true; 

            } else if (pauseScreen.enabled)
            {
                ResumeButton();
            }
        }
    }

    public void ResumeButton()
    {
        if (paused)
        {
            audioSource.PlayOneShot(clickSoundEffect, 0.5f);
            pauseScreen.enabled = false;
            if (!(turn == playerCount && hole == holeEnd)) playerScreen.enabled = true;
            if (reset)
            {
                NextTurn();
                reset = false;
            }
            else cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().isTurn = true;
            Time.timeScale = 1;
        }
        paused = false;
    }

    public void ControlsButton()
    {
        if (paused)
        {
            audioSource.PlayOneShot(clickSoundEffect, 0.5f);
            controlsScreen.enabled = true;
            pauseScreen.enabled = false;
        }
    }

    public void BackButton()
    {
        if (paused)
        {
            audioSource.PlayOneShot(clickSoundEffect, 0.5f);
            controlsScreen.enabled = false;
            pauseScreen.enabled = true;
        }
    }

    public void ResetBallButton()
    {
        if (paused)
        {
            if (cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().putts == maxPutts)
            {
                ForfeitHoleButton();
                return;
            }
            cameraPlayerTuples[turn - 1].Item2.SetActive(false);
            cameraPlayerTuples[turn - 1].Item2.GetComponent<Rigidbody>().velocity = Vector3.zero;
            cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().transform.eulerAngles = new Vector3(0f, -90f, 0f);
            cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().transform.position = holeLocations[hole];
            cameraPlayerTuples[turn - 1].Item2.SetActive(true);
            ResumeButton();
        }
    }

    public void ForfeitHoleButton()
    {
        if (paused)
        {
            cameraPlayerTuples[turn - 1].Item2.SetActive(false);
            cameraPlayerTuples[turn - 1].Item2.GetComponent<Rigidbody>().velocity = Vector3.zero;
            cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().totalPutts += maxPutts + maxPuttPenalty - cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().putts;
            cameraPlayerTuples[turn - 1].Item2.GetComponent<GolfBall>().putts = 0;
            playersFinished++;
            reset = true;
            ResumeButton();
        }
    }

    public void ExitGameButton()
    {
        if (paused || scoreboard.enabled)
        {
            audioSource.PlayOneShot(clickSoundEffect, 0.5f);
            SceneManager.LoadScene("Menu Scene");
        }
    }

    public void FillCameraPlayerTuples()
    {


        cameraPlayerTuples = new Tuple<Camera, GameObject>[playerCount];
        cameraPlayerTuples[0] = new Tuple<Camera, GameObject>(cam1, ball1);
        ball1.GetComponent<Renderer>().material.color = HexToColor(ballHexColors[0]);

        if (playerCount == 1) return;
        cameraPlayerTuples[1] = new Tuple<Camera, GameObject>(cam2, ball2);
        ball2.GetComponent<Renderer>().material.color = HexToColor(ballHexColors[1]);

        if (playerCount == 2) return;
        cameraPlayerTuples[2] = new Tuple<Camera, GameObject>(cam3, ball3);
        ball3.GetComponent<Renderer>().material.color = HexToColor(ballHexColors[2]);

        if (playerCount == 3) return;
        cameraPlayerTuples[3] = new Tuple<Camera, GameObject>(cam4, ball4);
        ball4.GetComponent<Renderer>().material.color = HexToColor(ballHexColors[3]);
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
            cameraPlayerTuples[prevTurn - 1].Item1.enabled = false;
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
            for (int j = 0; j < puttScores.Length; j++)
            {
                if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[j] && j == 0)
                {
                    firstPlaceNameTxt.text = playerNames[i];
                    firstPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();

                }
                else if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[j] && j == 1)
                {
                    secondPlaceNameTxt.text = playerNames[i];
                    secondPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();
                    secondPlaceNameTxt.enabled = true;
                    secondPlacePuttTxt.enabled = true;

                }
                else if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[j] && j == 2)
                {
                    thirdPlaceNameTxt.text = playerNames[i];
                    thirdPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();
                    thirdPlaceNameTxt.enabled = true;
                    thirdPlacePuttTxt.enabled = true;

                }
                else if (cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts == puttScores[j] && j == 3)
                {
                    fourthPlaceNameTxt.text = playerNames[i];
                    fourthPlacePuttTxt.text = cameraPlayerTuples[i].Item2.GetComponent<GolfBall>().totalPutts.ToString();
                    fourthPlaceNameTxt.enabled = true;
                    fourthPlacePuttTxt.enabled = true;

                }
            }
        }

        playerScreen.enabled = false;
        scoreboard.enabled = true;
    }

    Color HexToColor(string hex)
    {
        Color color = new Color();
        UnityEngine.ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}
