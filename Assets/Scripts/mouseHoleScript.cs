﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseHoleScript : MonoBehaviour
{
    public GameObject otherHole;
    GameObject buddyPlayer;
    public mouseHoleScript otherMSH;
    playerScript pS;
    // Start is called before the first frame update
    void Start()
    {
        buddyPlayer = GameObject.Find("Player");
        pS = buddyPlayer.GetComponent<playerScript>();
        otherMSH = otherHole.GetComponent<mouseHoleScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, otherHole.transform.position, Color.gray);
    }
}
