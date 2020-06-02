using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class postProcessMesserWither : MonoBehaviour
{

    godScript god;
    playerScript pS;
    
    // Start is called before the first frame update
    void Start()
    {
        god = GameObject.Find("GOD").GetComponent<godScript>();
        pS = GameObject.Find("Player").GetComponent<playerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
