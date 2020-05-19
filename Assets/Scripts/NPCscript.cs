using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCscript : MonoBehaviour
{
    godScript god;
    NavMeshAgent nav;
    public float fearAmount, fearDiminishRate, moveTimeMin, moveTimeMax, moveRangeMin, moveRangeMax;
    public bool scared, resetMove, screamed;
    public GameObject exit, buddyPlayer;
    playerScript pS;
    public ParticleSystem particlez;
    AudioSource aud;
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
        }
        else
        {
            scared = false;
            if(resetMove == false)
            {
                changePosition();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        RaycastHit rayHit;

        if(other.tag == "scary")
        {
            if (Physics.Raycast(transform.position, buddyPlayer.transform.position - transform.position, out rayHit, Vector3.Distance(transform.position, buddyPlayer.transform.position)))
            {
                Debug.DrawRay(transform.position, (buddyPlayer.transform.position - transform.position).normalized, Color.green, 0.1f);
                if(rayHit.transform.gameObject.tag == "Wall")
                {
                    return;
                }
                if (rayHit.transform.gameObject.tag == "Player")
                {
                    fearAmount = pS.spookResource + god.globalFearLevel;
                    if(screamed == false)
                    {
                        aud.pitch = Random.Range(1f, 3f);
                        aud.Play();
                        screamed = true;
                    }
                }
            }
        }
        if(other.tag == "Exit")
        {
            god.NPCcount--;
            Destroy(this.gameObject);
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
