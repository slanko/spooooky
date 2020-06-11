using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuFunctionsScript : MonoBehaviour
{
    Animator pauseMenuAnim;
    godScript GOD;

    private void Start()
    {
        pauseMenuAnim = GetComponent<Animator>();
        GOD = GameObject.Find("GOD").GetComponent<godScript>();
    }

    public void continueGame()
    {
        GOD.paused = false;
    }

    public void openOptions()
    {
        pauseMenuAnim.SetBool("optionsUp", true);
    }

    public void closeOptions()
    {
        pauseMenuAnim.SetBool("optionsUp", false);
    }

    public void exitToMenu()
    {
        SceneManager.LoadScene("mainMenuTest");
    }
}
