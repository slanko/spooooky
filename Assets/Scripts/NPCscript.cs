using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCscript : MonoBehaviour
{
    godScript god;
    NavMeshAgent nav;
    public float fearAmount, fearDiminishRate, moveTimeMin, moveTimeMax, moveRangeMin, moveRangeMax;
    public bool scared, resetMove, screamed, seenPlayer, subtractedFromSightCount;
    public GameObject exit, exit2, buddyPlayer, myScareSphere;
    GameObject preferredExit;
    playerScript pS;
    public ParticleSystem particlez;
    AudioSource aud;
    LineRenderer myLine;
    public LayerMask validLayers;
    Animator anim;
    Rigidbody rb;
    Vector3 oldPosition, newPosition;
    public float funnyVelocity;

    public GameObject model1, model2, modelFlashlight;
    public bool flashLightNpc;
    public Animator anim1, anim2, anim3;


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
        aud.volume = PlayerPrefs.GetFloat("audioVolume");
        if(flashLightNpc == false)
        {
            int randomNum = Random.Range(1, 3);
            if(randomNum == 1)
            {
                model1.SetActive(true);
                anim = anim1;
            }
            if(randomNum == 2)
            {
                model1.SetActive(false);
                model2.SetActive(true);
                anim = anim2;
            }
        }
        else
        {
           modelFlashlight.SetActive(true);
            model1.SetActive(false);
            anim = anim3;
        }
        oldPosition = transform.position;
    }

    private void Update()
    {

        figureOutVelocity();
        if(funnyVelocity < 0)
        {
            funnyVelocity = funnyVelocity * -1;
        }
        anim.SetFloat("runSpeed", funnyVelocity);
        Debug.DrawLine(transform.position, nav.destination, Color.green);
        if(scared == true)
        {
            bool exitPicked = false;
            if (exitPicked == false)
            {
                pickExit();
                exitPicked = true;
            }
            nav.SetDestination(preferredExit.transform.position);
            nav.speed = 10;
            if(particlez.isPlaying == false)
            {
                exitPicked = false;
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

        //update audio volume
        if(god != null)
        {
            aud.volume = god.globalAudioVolume;
        }

            if (Physics.Raycast(transform.position, buddyPlayer.transform.position - transform.position, out var rayHit, Vector3.Distance(transform.position, buddyPlayer.transform.position), validLayers))
            {
                if (rayHit.collider.gameObject.tag != "Player" || pS.cornMode == true)
                {
                    Debug.DrawRay(transform.position, (buddyPlayer.transform.position - transform.position), Color.red);
                    if (seenPlayer == true)
                    {
                        seenPlayer = false;
                        god.deregisterObserver(this.gameObject);
                        myLine.enabled = false;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, (buddyPlayer.transform.position - transform.position), Color.yellow);
                    if (seenPlayer == false)
                    {
                        god.registerObserver(this.gameObject);
                        seenPlayer = true;
                    }
                    myLine.enabled = true;
                    for (int i = 0; i < 10; i++)
                    {
                        myLine.SetPosition(i, Vector3.Lerp(transform.position, pS.gameObject.transform.position, i / 10f)); ;
                    }
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

    void pickExit()
    {
        float distToExit1, distToExit2;
        distToExit1 = Vector3.Distance(transform.position, exit.transform.position);
        distToExit2 = Vector3.Distance(transform.position, exit2.transform.position);
        if (distToExit1 > distToExit2)
        {
            preferredExit = exit2;
        }
        else
        {
            preferredExit = exit;
        }
    }

    void figureOutVelocity()
    {
        newPosition = transform.position;
        funnyVelocity = (newPosition.z - oldPosition.z) / Time.deltaTime;
        oldPosition = transform.position;
    }
}
