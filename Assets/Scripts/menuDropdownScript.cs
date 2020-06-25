using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuDropdownScript : MonoBehaviour
{
    public string levelToLoad;
    private void Start()
    {
        /* 
        if(playerChoice.monsterSelection == playerScript.monsterType.WIGGLYFELLA)
        {
            droppy.value = 0;
        }
        if (playerChoice.monsterSelection == playerScript.monsterType.BALLFELLA)
        {
            droppy.value = 1;
        }
        if (playerChoice.monsterSelection == playerScript.monsterType.SNAKEFELLA)
        {
            droppy.value = 2;
        }
        if (playerChoice.monsterSelection == playerScript.monsterType.CORNFELLA)
        {
            droppy.value = 3;
        }
        */
    }
    public void monsterSelect(int monsterNum)
    {
        if(monsterNum == 0)
        {
            playerChoice.monsterSelection = playerScript.monsterType.WIGGLYFELLA;
        }
        if (monsterNum == 1)
        {
            playerChoice.monsterSelection = playerScript.monsterType.BALLFELLA;
        }
        if (monsterNum == 2)
        {
            playerChoice.monsterSelection = playerScript.monsterType.SNAKEFELLA;
        }
        if (monsterNum == 3)
        {
            playerChoice.monsterSelection = playerScript.monsterType.CORNFELLA;
        }
        if(monsterNum == 69)
        {
            playerChoice.monsterSelection = playerScript.monsterType.SECRET1;
        }
        SceneManager.LoadScene(levelToLoad);
    }

}
