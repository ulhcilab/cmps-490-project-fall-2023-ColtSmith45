using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Canvas startMenu;
    public Canvas controlsScreen;
    public Canvas creditsScreen;
    public Canvas gameSetupScreen;
    private int holeStart = 1;
    private int holeEnd = 18;
    private int maxPutts = 8;
    private int maxPuttPenalty = 2;
    private int playerCount = 1; 
    public TextMeshProUGUI holeStartTxt;
    public TextMeshProUGUI holeEndTxt;
    public TextMeshProUGUI maxPuttsTxt;
    public TextMeshProUGUI maxPuttPenaltyTxt;
    public TextMeshProUGUI playerCountTxt;
    public TextMeshProUGUI colorSlider1Txt;
    public TextMeshProUGUI colorSlider2Txt;
    public TextMeshProUGUI colorSlider3Txt;
    public TextMeshProUGUI colorSlider4Txt;
    public Button holeStartPlus;
    public Button holeStartMinus;
    public Button holeEndPlus;
    public Button holeEndMinus;
    public Button maxPuttsPlus;
    public Button maxPuttsMinus;
    public Button maxPuttPenaltyPlus;
    public Button maxPuttPenaltyMinus;
    public Button playerCountPlus;
    public Button playerCountMinus;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public TMP_InputField player2Input;
    public TMP_InputField player3Input;
    public TMP_InputField player4Input;
    public AudioClip clickSoundEffect;
    private AudioSource audioSource;
    private Tuple<String, String>[] colorHexPairs = new Tuple<string, string>[]
    {
        Tuple.Create("White", "#FFFFFF"),
        Tuple.Create("Red", "#FF7F7F"),
        Tuple.Create("Orange", "#FFB366"),
        Tuple.Create("Yellow", "#FFFF66"),
        Tuple.Create("Green", "#7FFF7F"),
        Tuple.Create("Blue", "#7F7FFF"),
        Tuple.Create("Purple", "#B366FF"),
        Tuple.Create("Brown", "#8B4513"),
        Tuple.Create("Black", "#B3B3B3")
    };
    private string[] ballHexColors = {"#FFFFFF", "#FFFFFF", "#FFFFFF", "#FFFFFF"};
    private string[] playerNames = { "Player 1", "Player 2", "Player 3", "Player 4" };
    private int colorSlider1Index = 0;
    private int colorSlider2Index = 0;
    private int colorSlider3Index = 0;
    private int colorSlider4Index = 0;
    private float clickSoundLevel = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //initialize screens enabled/disable states
        controlsScreen.enabled = false;
        creditsScreen.enabled = false;
        gameSetupScreen.enabled = false;
        startMenu.enabled = true; 

        //Hide Non-added player boxes 
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);

        //Assign default gamesetup values 
        holeStartTxt.text = holeStart.ToString();
        holeEndTxt.text = holeEnd.ToString();
        maxPuttsTxt.text = maxPutts.ToString();
        maxPuttPenaltyTxt.text = maxPuttPenalty.ToString();
        playerCountTxt.text = playerCount.ToString();

        //Assign starting gray arrows 
        holeStartMinus.image.color = Color.gray;
        holeEndPlus.image.color = Color.gray;
        playerCountMinus.image.color = Color.gray;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BackButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (controlsScreen.enabled)
        {
            controlsScreen.enabled = false;

        } else if (creditsScreen.enabled)
        {
            creditsScreen.enabled = false;

        } else if (gameSetupScreen.enabled)
        {
            gameSetupScreen.enabled = false;
        }
        startMenu.enabled = true;
    }

    public void ControlsButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        startMenu.enabled = false;
        controlsScreen.enabled = true;
    }

    public void CreditsButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        startMenu.enabled = false;
        creditsScreen.enabled = true;
    }

    public void PlayButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        startMenu.enabled = false;
        gameSetupScreen.enabled = true;
    }

    public void ExitButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void HoleStartMinusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (holeStart != 1)
        {
            holeStart--;
            holeStartTxt.text = holeStart.ToString();
            if (holeStart == 1) holeStartMinus.image.color = Color.gray;
            else
            {
                holeStartMinus.image.color = Color.white;
                holeStartPlus.image.color = Color.white;
                holeEndMinus.image.color = Color.white;
            }
        } 
    }
    public void HoleStartPlusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (holeStart != holeEnd)
        {
            holeStart++;
            holeStartTxt.text = holeStart.ToString();
            holeStartMinus.image.color = Color.white;
            if (holeStart == holeEnd)
            {
                holeStartPlus.image.color = Color.gray;
                holeEndMinus.image.color = Color.gray;
            }
            else holeStartPlus.image.color = Color.white;
        }
    }


    public void HoleEndMinusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (holeEnd != 1 && holeEnd != holeStart)
        {
            holeEnd--;
            holeEndTxt.text = holeEnd.ToString();
            holeEndPlus.image.color = Color.white;
            if (holeEnd == holeStart)
            {
                holeEndMinus.image.color = Color.gray;
                holeStartPlus.image.color = Color.gray;
            }
            else holeEndMinus.image.color = Color.white;
        }
    }
    public void HoleEndPlusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (holeEnd != 18)
        {
            holeEnd++;
            holeEndTxt.text = holeEnd.ToString();
            holeEndMinus.image.color = Color.white;
            if (holeEnd == 18)
            {
                holeEndPlus.image.color = Color.gray;
                holeStartPlus.image.color = Color.white;
            }
            else
            {
                holeEndPlus.image.color = Color.white;
                holeStartPlus.image.color = Color.white;
            }
        }
    }


    public void MaxPuttsMinusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (maxPutts != 1)
        {
            maxPutts--;
            maxPuttsTxt.text = maxPutts.ToString();
            maxPuttsPlus.image.color = Color.white;
            if (maxPutts == 1) maxPuttsMinus.image.color = Color.gray;
        }
    }
    public void MaxPuttsPlusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (maxPutts != 10)
        {
            maxPutts++;
            maxPuttsTxt.text = maxPutts.ToString();
            maxPuttsMinus.image.color = Color.white;
            if (maxPutts == 10) maxPuttsPlus.image.color = Color.gray;
        }
    }


    public void MaxPuttPenaltyMinusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (maxPuttPenalty != 1)
        {
            maxPuttPenalty--;
            maxPuttPenaltyTxt.text = maxPuttPenalty.ToString();
            maxPuttPenaltyPlus.image.color = Color.white;
            if (maxPuttPenalty == 1) maxPuttPenaltyMinus.image.color = Color.gray;
        }
    }
    public void MaxPuttPenaltyPlusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (maxPuttPenalty != 3)
        {
            maxPuttPenalty++;
            maxPuttPenaltyTxt.text = maxPuttPenalty.ToString();
            maxPuttPenaltyMinus.image.color = Color.white;
            if (maxPuttPenalty == 3) maxPuttPenaltyPlus.image.color = Color.gray;
        }
    }


    public void PlayerCountMinusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (playerCount != 1)
        {
            playerCount--;
            playerCountTxt.text = playerCount.ToString();
            playerCountPlus.image.color = Color.white;
            if (playerCount == 1)
            {
                playerCountMinus.image.color = Color.gray;
                player2.SetActive(false);
                ballHexColors[1] = "#FFFFFF";
                colorSlider2Index = 0;
                colorSlider2Txt.text = colorHexPairs[0].Item1;
                playerNames[1] = "Player 2";
                player2Input.text = "";
            } else if (playerCount == 2)
            {
                player3.SetActive(false);
                ballHexColors[2] = "#FFFFFF";
                colorSlider3Index = 0;
                colorSlider3Txt.text = colorHexPairs[0].Item1;
                playerNames[2] = "Player 3";
                player3Input.text = "";
            } else if (playerCount == 3)
            {
                player4.SetActive(false);
                ballHexColors[3] = "#FFFFFF";
                colorSlider4Index = 0;
                colorSlider4Txt.text = colorHexPairs[0].Item1;
                playerNames[3] = "Player 4";
                player4Input.text = "";
            }
        }
    }
    public void PlayerCountPlusButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        if (playerCount != 4)
        {
            playerCount++;
            playerCountTxt.text = playerCount.ToString();
            playerCountMinus.image.color = Color.white;

            if (playerCount == 2)
            {
                player2.SetActive(true);

            } else if (playerCount == 3)
            {
                player3.SetActive(true);
            } else if (playerCount == 4)
            {
                if (playerCount == 4) playerCountPlus.image.color = Color.gray;
                player4.SetActive(true);

            }
        }
    }


    public void RightColorArrowButton1()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider1Index++;
        if (colorSlider1Index == colorHexPairs.Length)
        {
            colorSlider1Index = 0;
        }
        colorSlider1Txt.text = colorHexPairs[colorSlider1Index].Item1;
        ballHexColors[0] = colorHexPairs[colorSlider1Index].Item2;

    }

    public void LeftColorArrowButton1()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider1Index--;
        if (colorSlider1Index == -1)
        {
            colorSlider1Index = colorHexPairs.Length - 1;
        }
        colorSlider1Txt.text = colorHexPairs[colorSlider1Index].Item1;
        ballHexColors[0] = colorHexPairs[colorSlider1Index].Item2;

    }


    public void RightColorArrowButton2()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider2Index++;
        if (colorSlider2Index == colorHexPairs.Length)
        {
            colorSlider2Index = 0;
        }
        colorSlider2Txt.text = colorHexPairs[colorSlider2Index].Item1;
        ballHexColors[1] = colorHexPairs[colorSlider2Index].Item2;

    }

    public void LeftColorArrowButton2()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider2Index--;
        if (colorSlider2Index == -1)
        {
            colorSlider2Index = colorHexPairs.Length - 1;
        }
        colorSlider2Txt.text = colorHexPairs[colorSlider2Index].Item1;
        ballHexColors[1] = colorHexPairs[colorSlider2Index].Item2;

    }


    public void RightColorArrowButton3()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider3Index++;
        if (colorSlider3Index == colorHexPairs.Length)
        {
            colorSlider3Index = 0;
        }
        colorSlider3Txt.text = colorHexPairs[colorSlider3Index].Item1;
        ballHexColors[2] = colorHexPairs[colorSlider3Index].Item2;

    }

    public void LeftColorArrowButton3()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider3Index--;
        if (colorSlider3Index == -1)
        {
            colorSlider3Index = colorHexPairs.Length - 1;
        }
        colorSlider3Txt.text = colorHexPairs[colorSlider3Index].Item1;
        ballHexColors[2] = colorHexPairs[colorSlider3Index].Item2;

    }


    public void RightColorArrowButton4()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider4Index++;
        if (colorSlider4Index == colorHexPairs.Length)
        {
            colorSlider4Index = 0;
        }
        colorSlider4Txt.text = colorHexPairs[colorSlider4Index].Item1;
        ballHexColors[3] = colorHexPairs[colorSlider4Index].Item2;
    }

    public void LeftColorArrowButton4()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        colorSlider4Index--;
        if (colorSlider4Index == -1)
        {
            colorSlider4Index = colorHexPairs.Length - 1;
        }
        colorSlider4Txt.text = colorHexPairs[colorSlider4Index].Item1;
        ballHexColors[3] = colorHexPairs[colorSlider4Index].Item2;

    }


    public void grabPlayer1Name(string name)
    {
        playerNames[0] = name;
    }

    public void grabPlayer2Name(string name)
    {
        playerNames[1] = name;
    }

    public void grabPlayer3Name(string name)
    {
        playerNames[2] = name;
    }

    public void grabPlayer4Name(string name)
    {
        playerNames[3] = name;
    }



    public void StartGameButton()
    {
        audioSource.PlayOneShot(clickSoundEffect, clickSoundLevel);
        TurnManager.holeStart = holeStart;
        TurnManager.holeEnd = holeEnd;
        TurnManager.maxPutts = maxPutts;
        TurnManager.maxPuttPenalty = maxPuttPenalty;
        TurnManager.playerCount = playerCount;
        TurnManager.playersFinished = 0;
        TurnManager.ballHexColors = ballHexColors;
        TurnManager.playerNames = playerNames;
        SceneManager.LoadScene("Game Scene");
    }





}
