using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using System;
public class godScript : MonoBehaviour
{
    [Header("NPC stuff")]
    public float NPCcount, globalFearLevel;

    [Header("Game Control Stuff")]
    public GameObject winText;
    public Text counter;
    bool invokedRestart = false;

    [Header("Score Stuff")]
    public float scoreCountdownWaitTime;
    public float currentCountdownTime;
    public float softScore, loggedScore, scoreMultiplier;
    public int sendScore;
    public Text softscoreCounter, ScoreCounter, timerText;
    public TimeSpan gameTime;
    public float timeScoreMult, timeScoreMultDecay, timeScoreMultMin;

    [Header("Audio Stuff")]
    public KeyCode musicMuteButton;
    public AudioSource muzik;
    public float musicVolume;
    public bool musicMuted;

    // Start is called before the first frame update
    void Start()
    {
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Test Level");
        muzik.volume = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if(NPCcount == 0)
        {
            winText.SetActive(true);
            if(invokedRestart == false)
            {
                invokedRestart = true;
                logScore();
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Test Level", sendScore);
                Invoke("resetGame", 3);
                muzik.volume = musicVolume;
            }
        }
        if(currentCountdownTime > 0)
        {
            currentCountdownTime = currentCountdownTime - Time.deltaTime;
        }
        if(currentCountdownTime < 0)
        {
            currentCountdownTime = 0;
            logScore();
        }
        ScoreCounter.text = "" + loggedScore;
        if(softScore > 0)
        {
            softscoreCounter.text = "+" + softScore;
        }
        else
        {
            softscoreCounter.text = null;
        }

        //timer stuff
        if (NPCcount > 0)
        {
            gameTime += TimeSpan.FromSeconds(Time.deltaTime);
            timerText.text = gameTime.ToString(@"mm\:ss\.ff");
            timeScoreMult = timeScoreMult - (Time.deltaTime * timeScoreMultDecay);
            if (timeScoreMult < timeScoreMultMin)
            {
                timeScoreMult = timeScoreMultMin;
            }
        }

        //goto menu stuff
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenuTest");
        }

        //music mute stuff
        if (Input.GetKeyDown(musicMuteButton))
        {
            if(musicMuted == false)
            {
                muzik.volume = 0;
                musicMuted = true;
            }
            else
            {
                muzik.volume = musicVolume;
                musicMuted = false;
            }
        }

        counter.text = NPCcount.ToString();
        sendScore = (int) loggedScore;
    }

    void resetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void upScore(int scoreAmount, int multAmount)
    {
        scoreMultiplier = scoreMultiplier + multAmount;
        softScore = Mathf.Ceil((softScore + (scoreAmount * scoreMultiplier)) * timeScoreMult);
        currentCountdownTime = scoreCountdownWaitTime;
    }

    void logScore()
    {
        loggedScore = loggedScore + softScore;
        softScore = 0;
        scoreMultiplier = 0;
    }
}
