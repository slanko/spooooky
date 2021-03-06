﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCscript : MonoBehaviour
{
    godScript god;
    NavMeshAgent nav;
    public float fearAmount, fearDiminishRate, moveTimeMin, moveTimeMax, moveRangeMin, moveRangeMax;
    public bool scared, resetMove, screamed, seenPlayer, subtractedFromSightCount;
    public GameObject exit, buddyPlayer, myScareSphere;
    playerScript pS;
    public ParticleSystem particlez;
    AudioSource aud;
    LineRenderer myLine;


    [Header("Score Stuff")]
    public int pointValue;
    public int multiplierChange;
    // Start is called before the first frame update
    void Start()
    {
        god = GameObject.Find("GOD").GetComponent<godScript>();
        nav = GetComponent<NavMeshAgent>();
        changePosition();
        exit = GameObject.Find("Exit");
        aud = GetComponent<AudioSource>();
        pS = GameObject.Find("Player").GetComponent<playerScript>();
        buddyPlayer = GameObject.Find("Player");
        myLine = GetComponent<LineRenderer>();
        god.NPCcount++;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, nav.destination, Color.green);

        if(scared == true)
        {
            nav.SetDestination(exit.transform.position);
            nav.speed = 10;
            if(particlez.isPlaying == false)
            {
                particlez.Play();
            }
        }
        else
        {
            if(particlez.isPlaying == true)
            {
                particlez.Stop();
            }
        }
        if(fearAmount > 0)
        {
            fearAmount = fearAmount - (fearDiminishRate * Time.deltaTime);
            scared = true;
            resetMove = false;
            myScareSphere.SetActive(true);
        }
        else
        {
            scared = false;
            myScareSphere.SetActive(false);
            if (resetMove == false)
            {
                changePosition();
            }
        }

        if (Physics.Raycast(transform.position, buddyPlayer.transform.position - transform.position, out var rayHit, Vector3.Distance(transform.position, buddyPlayer.transform.position)))
        {
            if(scared == false)
            {
                if (rayHit.collider.gameObject.tag != "Player")
                {
                    Debug.DrawRay(transform.position, (buddyPlayer.transform.position - transform.position), Color.red);
                    if(seenPlayer == true)
                    {
                        seenPlayer = false;
                        god.deregisterObserver(this.gameObject);
                        myLine.enabled = false;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, (buddyPlayer.transform.position - transform.position), Color.yellow);
                    if(seenPlayer == false)
                    {
                        god.registerObserver(this.gameObject);
                        seenPlayer = true;
                    }
                    myLine.enabled = true;
                    myLine.SetPosition(0, transform.position);
                    myLine.SetPosition(1, pS.gameObject.transform.position);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, (buddyPlayer.transform.position - transform.position), Color.blue);
                myLine.enabled = false;
                god.deregisterObserver(this.gameObject);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if the other tag is a scary radius
        if(other.tag == "scary" || other.tag == "scareSphereAlt")
        {
            //shoot a raycast out to determine the player distance, and get a raycast hit on whatever it hits
            if (Physics.Raycast(transform.position, buddyPlayer.transform.position - transform.position, out var rayHit, Vector3.Distance(transform.position, buddyPlayer.transform.position)))
            {
                if(rayHit.collider.gameObject.tag != "Player")
                {
                    Debug.DrawRay(transform.position, (buddyPlayer.transform.position - transform.position), Color.red, 5f);
                    return;
                }
                else 
                {
                    Debug.DrawLine(transform.position, rayHit.point, Color.cyan, 5f);
                    if (pS.spookResource > 0 && pS.stealthed == false)
                    {
                        fearAmount = pS.spookResource + god.globalFearLevel;
                        if (screamed == false)
                        {
                            aud.pitch = Random.Range(1f, 3f);
                            aud.Play();
                            screamed = true;
                        }
                    }
                }
            }
        }
        if(other.tag == "Exit")
        {
            god.NPCcount--;
            god.upScore(pointValue, multiplierChange);
            god.deregisterObserver(this.gameObject);
            Destroy(this.gameObject);
        }

        if(other.tag == "scareSphere")
        {
            if (other.gameObject == myScareSphere)
            {
                return;
            }
            else
            {
                if (other.transform.parent == null)
                {
                    scareSphereScript scareSphere = other.GetComponent<scareSphereScript>();
                    fearAmount = scareSphere.scarinessLevel;
                    if (screamed == false)
                    {
                        aud.pitch = Random.Range(1f, 3f);
                        aud.Play();
                        screamed = true;
                    }
                }
                else if(other.transform.parent.tag == "NPC")
                {
                    NPCscript otherNPCbrain;
                    otherNPCbrain = other.gameObject.GetComponentInParent<NPCscript>();
                    if (otherNPCbrain.scared == true)
                    {
                        fearAmount = otherNPCbrain.fearAmount;
                        if (screamed == false)
                        {
                            aud.pitch = Random.Range(1f, 3f);
                            aud.Play();
                            screamed = true;
                        }
                    }
                }
            }
        }
    }

    void changePosition()
    {
        if(scared == false)
        {
            screamed = false;
            nav.speed = 3.5f;
            resetMove = true;
            Vector3 navPos;
            navPos = Random.insideUnitSphere * Random.Range(moveRangeMin, moveRangeMax) + transform.position;
            navPos = new Vector3(navPos.x, 0, navPos.z);
            nav.SetDestination(navPos);
            Invoke("changePosition", Random.Range(moveTimeMin, moveTimeMax));
        }
    }
}
