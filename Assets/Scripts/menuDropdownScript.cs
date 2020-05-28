using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuDropdownScript : MonoBehaviour
{
    Dropdown droppy;
    private void Start()
    {
        droppy = GetComponent<Dropdown>();
    }
    public void monsterSelect()
    {
        if(droppy.value == 0)
        {
            playerChoice.monsterSelection = playerScript.monsterType.WIGGLYFELLA;
        }
        if (droppy.value == 1)
        {
            playerChoice.monsterSelection = playerScript.monsterType.BALLFELLA;
        }
        if (droppy.value == 2)
        {
            playerChoice.monsterSelection = playerScript.monsterType.SNAKEFELLA;
        }
        if (droppy.value == 3)
        {
            playerChoice.monsterSelection = playerScript.monsterType.CORNFELLA;
        }
    }

}
