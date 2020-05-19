using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
public class godScript : MonoBehaviour
{
    public float NPCcount, globalFearLevel;
    public GameObject winText;
    public Text counter;
    bool invokedRestart = false;
    // Start is called before the first frame update
    void Start()
    {
        GameAnalytics.Initialize();
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
                Invoke("resetGame", 3);
            }
        }

        counter.text = NPCcount.ToString();
    }

    void resetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
