using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCscript : MonoBehaviour
{
    godScript god;
    NavMeshAgent nav;
    public float fearAmount, fearDiminishRate, moveTimeMin, moveTimeMax, moveRangeMin, moveRangeMax;
    public bool scared, resetMove;
    public GameObject exit;
    playerScript pS;
    public ParticleSystem particlez;
    // Start is called before the first frame update
    void Start()
    {
        god = GameObject.Find("GOD").GetComponent<godScript>();
        nav = GetComponent<NavMeshAgent>();
        changePosition();
        exit = GameObject.Find("Exit");
        pS = GameObject.Find("Player").GetComponent<playerScript>();
        god.NPCcount++;
    }

    private void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "scary")
        {
            fearAmount = pS.spookResource;
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
