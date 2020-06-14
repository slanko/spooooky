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

    public void swapControlsOn()
    {
        pauseMenuAnim.SetBool("controlsSwap", true);
    }

    public void swapControlsOff()
    {
        pauseMenuAnim.SetBool("controlsSwap", false);
    }

    public void openControls()
    {
        pauseMenuAnim.SetBool("controlsUp", true);
        pauseMenuAnim.SetBool("controlsSwap", false);
    }

    public void closeControls()
    {
        pauseMenuAnim.SetBool("controlsUp", false);
        pauseMenuAnim.SetBool("controlsSwap", false);
    }
}
