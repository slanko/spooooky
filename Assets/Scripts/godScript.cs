using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
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
    public int softScore, loggedScore, scoreMultiplier;
    public Text softscoreCounter, ScoreCounter;

    // Start is called before the first frame update
    void Start()
    {
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Test Level");
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
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Test Level", loggedScore);
                Invoke("resetGame", 3);
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

        counter.text = NPCcount.ToString();
    }

    void resetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void upScore(int scoreAmount, int multAmount)
    {
        scoreMultiplier = scoreMultiplier + multAmount;
        softScore = softScore + (scoreAmount * scoreMultiplier);
        currentCountdownTime = scoreCountdownWaitTime;
    }

    void logScore()
    {
        loggedScore = loggedScore + softScore;
        softScore = 0;
        scoreMultiplier = 0;
    }
}
