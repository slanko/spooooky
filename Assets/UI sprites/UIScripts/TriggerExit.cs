using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExit : MonoBehaviour
{
    public GameObject playerSelect;
    public GameObject mainMenu;
    public GameObject settingsScreen;
    public Animator anim;
    public Animator anim2;
    public Animator anim3;

    public void PlayPressed()
    {
        anim.SetTrigger("Play");
    }

    public void BackPressed()
    {
        anim2.SetTrigger("Back");
    }
    public void SettingsPressed()
    {
        anim.SetTrigger("Settings");
    }

    public void SettingsBackPressed()
    {
        anim3.SetTrigger("Back");
    }

    public void menuTrigger(string menuToTransitionTo)
    {
        if(menuToTransitionTo == "playerSelect")
        {
            playerSelect.SetActive(true);
            mainMenu.SetActive(false);
        }
        if(menuToTransitionTo == "mainMenu")
        {
            playerSelect.SetActive(false);
            mainMenu.SetActive(true);
            settingsScreen.SetActive(false);
            anim.SetTrigger("Entry");
        }
        if(menuToTransitionTo == "settingsScreen")
        {
            playerSelect.SetActive(false);
            mainMenu.SetActive(false);
            settingsScreen.SetActive(true);
            anim.SetTrigger("Entry");
        }
    }
}
