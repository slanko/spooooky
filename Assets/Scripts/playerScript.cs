using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class playerScript : MonoBehaviour
{
    Animator anim, anim2;
    AudioSource aud;
    godScript god;

    public monsterType monsterSelection;
    [Header("Wiggly Fella Values"), Tooltip("Values for the Wiggly Fella. Essentially default, all rounder character with a special movement option.")]
    public float monster1MoveSpeed;
    public float monster1ScareRadius, monster1BlockRadius, monster1SpookGain, monster1SpookDecay;
    public GameObject monster1Model;

    [Header("Ball Fella Values"), Tooltip("Values for the Funny Little Ball Fella. A fast little fella who isn't exactly scary but has SPEED abilities to back it up.")]
    public float monster2MoveSpeed;
    public float monster2ScareRadius, monster2BlockRadius, monster2SpookGain, monster2SpookDecay;
    public GameObject monster2Model;

    [Header("Snake Fella Values"), Tooltip("Values for the Snake Fella. ")]
    public float monster3MoveSpeed;
    public float monster3ScareRadius, monster3BlockRadius, monster3SpookGain, monster3SpookDecay;
    public GameObject monster3Model;

    [Header("Corn Fella Values"), Tooltip("Values for the Snake Fella. ")]
    public float monster4MoveSpeed;
    public float monster4ScareRadius, monster4BlockRadius, monster4SpookGain, monster4SpookDecay;
    public GameObject monster4Model;

    [Header("Global Inputs")]
    public KeyCode scareKey;
    public KeyCode useKey, camLeftKey, camRightKey, resetGameKey;
    public float moveSpeed;
    private float moveSpeedFactor; 

    [Header("Spook Mechanics")]
    public Animator playerIconAnim;
    public bool stealthed;
    public Slider spookOMeter;
    public float spookResource, spookGainRate, spookDiminishRate;
    public SphereCollider scareRadius;
    public float scareRadiusMult;

    [Header("Camera Stuff")]
    public float camLerpSpeed;
    public GameObject cameraBuddy, cameraBuddyBuddy, debugCircleVisualizer1;
    bool camStickRotated;

    [Header("AI Stuff")]
    public NavMeshObstacle navBlocker;

    [Header("Ability Stuff")]
    public slimeTrailAbility slimy;
    public KeyCode toggleSlime;

    [Header("Fun Stuff")]
    public bool firstPersonMode;
    public GameObject firstPersonCam;

    private bool IsMoving;

    [System.Serializable]
    public enum monsterType
    {
        WIGGLYFELLA,
        BALLFELLA,
        SNAKEFELLA,
        CORNFELLA
    }
    // Start is called before the first frame update
    void Start()
    {
        god = GameObject.Find("GOD").GetComponent<godScript>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        slimy = GetComponent<slimeTrailAbility>();

        if(playerChoice.monsterSelection == monsterType.WIGGLYFELLA)
        {
            moveSpeed = monster1MoveSpeed;
            scareRadius.radius = monster1ScareRadius;
            scareRadiusMult = monster1BlockRadius;
            spookGainRate = monster1SpookGain;
            spookDiminishRate = monster1SpookDecay;
            monster1Model.SetActive(true);
            monster2Model.SetActive(false);
            monster3Model.SetActive(false);
            monster4Model.SetActive(false);
            anim2 = monster1Model.GetComponent<Animator>();
        }
        if (playerChoice.monsterSelection == monsterType.BALLFELLA)
        {
            moveSpeed = monster2MoveSpeed;
            scareRadius.radius = monster2ScareRadius;
            scareRadiusMult = monster2BlockRadius;
            spookGainRate = monster2SpookGain;
            spookDiminishRate = monster2SpookDecay;
            monster1Model.SetActive(false);
            monster2Model.SetActive(true);
            monster3Model.SetActive(false);
            monster4Model.SetActive(false);
            anim2 = monster2Model.GetComponent<Animator>();
        }
        if (playerChoice.monsterSelection == monsterType.SNAKEFELLA)
        {
            moveSpeed = monster3MoveSpeed;
            scareRadius.radius = monster3ScareRadius;
            scareRadiusMult = monster3BlockRadius;
            spookGainRate = monster3SpookGain;
            spookDiminishRate = monster3SpookDecay;
            monster1Model.SetActive(false);
            monster2Model.SetActive(false);
            monster3Model.SetActive(true);
            monster4Model.SetActive(false);
            anim2 = monster3Model.GetComponent<Animator>();
        }
        if (playerChoice.monsterSelection == monsterType.CORNFELLA)
        {
            moveSpeed = monster4MoveSpeed;
            scareRadius.radius = monster4ScareRadius;
            scareRadiusMult = monster4BlockRadius;
            spookGainRate = monster4SpookGain;
            spookDiminishRate = monster4SpookDecay;
            monster1Model.SetActive(false);
            monster2Model.SetActive(false);
            monster3Model.SetActive(false);
            monster4Model.SetActive(true);
            anim2 = monster4Model.GetComponent<Animator>();
        }

        moveSpeedFactor = 1;
        anim2.speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        var vert = Input.GetAxisRaw("Vertical");
        var horiz = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horiz, 0, vert);
        IsMoving = movement.magnitude > 0;
        var rotation = new Quaternion(0,0,0,0);
        if (firstPersonMode == false)
        {
            rotation = Quaternion.LookRotation(cameraBuddy.transform.forward, Vector3.up);
        }

        var realMove = rotation * movement;

        transform.LookAt(transform.position + realMove);
        //end big fat movement code block

        //dumb first person mode start
        if(firstPersonMode == true)
        {
            firstPersonCam.SetActive(true);
        }
        else
        {
            firstPersonCam.SetActive(false);
        }


        //movement animations bit
        if(IsMoving == true)
        {
            anim2.SetBool("walking", true);
        }
        else
        {
            anim2.SetBool("walking", false);
        }

        //lovely actions section
        if (Input.GetKeyDown(scareKey) || Input.GetButtonDown("Scare"))
        {
            if (stealthed == false)
            {
                anim.SetTrigger("scare");
                anim2.SetTrigger("spook");
                aud.pitch = Random.Range(.5f, 1.5f);
                aud.Play();
            }
        }

        //debug slime trail enable/disable
        if(playerChoice.monsterSelection == monsterType.SNAKEFELLA)
        {
            if (Input.GetKeyDown(toggleSlime))
            {
                if (slimy.weOutHereSliming == false)
                {
                    slimy.weOutHereSliming = true;
                }
                else
                {
                    slimy.weOutHereSliming = false;
                }
            }
        }


        //camera controls part, this changes the cameraBuddy's Buddy's position so the camera buddy can lerp to it's rotation (it's an easier but slightly messy way OF doing it)
        if (Input.GetKeyDown(camLeftKey))
        {
            cameraBuddyBuddy.transform.Rotate(0, -90, 0);
        }
        if (Input.GetKeyDown(camRightKey))
        {
            cameraBuddyBuddy.transform.Rotate(0, 90, 0);
        }

        if(camStickRotated == false)
        {
            if (Input.GetAxis("CamHorizontal") > 0.5)
            {
                cameraBuddyBuddy.transform.Rotate(0, -90, 0);
            }
            if (Input.GetAxis("CamHorizontal") < -0.5)
            {
                cameraBuddyBuddy.transform.Rotate(0, 90, 0);
            }
            camStickRotated = true;
        }

        if (Input.GetAxis("CamHorizontal") < 0.4 && Input.GetAxis("CamHorizontal") > -0.4)
        {
            camStickRotated = false;
        }


        //reset game input
        if (Input.GetKeyDown(resetGameKey))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        //stealth checks &  value changes
        if (stealthed == false)
        {
            playerIconAnim.SetBool("amSeen", true);
            if (spookResource > 0)
            {
                spookResource = spookResource - (spookDiminishRate * Time.deltaTime);
            }
            else
            {
                spookResource = 0;
            }
        }
        if(stealthed == true)
        {
            playerIconAnim.SetBool("amSeen", false);
            if (spookResource < spookOMeter.maxValue)
            {
                spookResource = spookResource + (spookGainRate * Time.deltaTime);
            }
            if (spookResource > spookOMeter.maxValue)
            {
                spookResource = spookOMeter.maxValue;
            }
        }

        //line-of-sight stealthy stuffe
        stealthed = !god.isSeen();

        //nav blocker size change based on scariness
        navBlocker.radius = (spookResource / 3) * scareRadiusMult;
        float circleVizSize = (spookResource / 3) * scareRadiusMult/10;
        debugCircleVisualizer1.transform.localScale =new Vector3(circleVizSize, circleVizSize, circleVizSize);

        spookOMeter.value = spookResource;

        //player icon animation stuff
        playerIconAnim.SetInteger("scaryLevel", Mathf.CeilToInt(spookResource));
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            transform.Translate(Vector3.forward * (moveSpeed * moveSpeedFactor) * Time.deltaTime);
        }
        cameraBuddy.transform.position = Vector3.Lerp(cameraBuddy.transform.position, transform.position, camLerpSpeed);
        cameraBuddy.transform.rotation = Quaternion.Slerp(cameraBuddy.transform.rotation, cameraBuddyBuddy.transform.rotation, camLerpSpeed);
        cameraBuddyBuddy.transform.position = transform.position;
    }

    public void setSpeedFactor(float factor)
    {
        moveSpeedFactor = factor;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "stealthzone")
        {
            stealthed = true;
        }
        if(other.tag == "mouseHole")
        {
            if(playerChoice.monsterSelection == monsterType.WIGGLYFELLA)
            {
                if (Input.GetKeyDown(useKey) || Input.GetButtonDown("Interact"))
                {
                    mouseHoleScript MSH;
                    MSH = other.GetComponent<mouseHoleScript>();
                    transform.position = new Vector3(MSH.otherMSH.transform.position.x, transform.position.y, MSH.otherMSH.transform.position.z);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "stealthzone")
        {
            stealthed = false;
        }
    }
}
