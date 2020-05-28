using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleVisualiserScript : MonoBehaviour
{
    public playerScript pS;
    // Start is called before the first frame update
    void Start()
    {
        if(playerChoice.monsterSelection == playerScript.monsterType.WIGGLYFELLA)
        {
            transform.localScale = new Vector3(0.52f, 0.52f, 0.52f);
        }
        if(playerChoice.monsterSelection == playerScript.monsterType.BALLFELLA)
        {
            transform.localScale = new Vector3(0.29f, 0.29f, 0.29f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
