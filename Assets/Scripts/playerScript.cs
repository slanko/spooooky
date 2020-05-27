using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class playerScript : MonoBehaviour
{
    Animator anim;
    AudioSource aud;

    public int monsterType;
    [Header("Monster 1 Values"), Tooltip("Values for the Wiggly Fella. Essentially default, all rounder character with a special movement option.")]
    public float monster1MoveSpeed;
    public float monster1ScareRadius, monster1BlockRadius, monster1SpookDecay;

    [Header("Monster 2 Values"), Tooltip("Values for the Funny Little Ball Fella. A fast little fella who isn't exactly scary but has SPEED abilities to back it up.")]
    public float monster2MoveSpeed;
    public float monster2ScareRadius, monster2BlockRadius, monster2SpookDecay;

    [Header("Global Inputs")]
    public KeyCode scareKey;
    public KeyCode useKey, camLeftKey, camRightKey, resetGameKey;
    public float moveSpeed;

    [Header("Spook Mechanics")]
    public bool stealthed;
    public Slider spookOMeter;
    public float spookResource, spookGainRate, spookDiminishRate;
    public SphereCollider scareRadius;
    public float scareRadiusMult;

    [Header("Camera Stuff")]
    public float camLerpSpeed;
    public GameObject cameraBuddy, cameraBuddyBuddy, debugCircleVisualizer1;

    [Header("AI Stuff")]
    public NavMeshObstacle navBlocker;

    [Header("Ability Stuff")]
    public slimeTrailAbility slimy;
    public KeyCode toggleSlime;

    private bool IsMoving;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        slimy = GetComponent<slimeTrailAbility>();

        if(monsterType == 1)
        {
            moveSpeed = monster1MoveSpeed;
            scareRadius.radius = monster1ScareRadius;
            scareRadiusMult = monster1BlockRadius;
            spookDiminishRate = monster1SpookDecay;
        }
        if (monsterType == 2)
        {
            moveSpeed = monster2MoveSpeed;
            scareRadius.radius = monster2ScareRadius;
            scareRadiusMult = monster2BlockRadius;
            spookDiminishRate = monster2SpookDecay;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var vert = Input.GetAxisRaw("Vertical");
        var horiz = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horiz, 0, vert);
        IsMoving = movement.magnitude > 0;
        var rotation = Quaternion.LookRotation(cameraBuddy.transform.forward, Vector3.up);

        var realMove = rotation * movement;

        transform.LookAt(transform.position + realMove);


        //end big fat movement code block

        //lovely actions section
        if (Input.GetKeyDown(scareKey) || Input.GetButtonDown("Scare"))
        {
            if (stealthed == false)
            {
                anim.SetTrigger("scare");
                aud.pitch = Random.Range(.5f, 1.5f);
                aud.Play();
            }
        }

        //debug slime trail enable/disable
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

        //camera controls part, this changes the cameraBuddy's Buddy's position so the camera buddy can lerp to it's rotation (it's an easier but slightly messy way OF doing it)
        if (Input.GetKeyDown(camLeftKey) || Input.GetButtonDown("CamLeft"))
        {
            cameraBuddyBuddy.transform.Rotate(0, -90, 0);
        }
        if (Input.GetKeyDown(camRightKey) || Input.GetButtonDown("CamRight"))
        {
            cameraBuddyBuddy.transform.Rotate(0, 90, 0);
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
            if(spookResource < spookOMeter.maxValue)
            {
                spookResource = spookResource + (spookGainRate * Time.deltaTime);
            }
            if (spookResource > spookOMeter.maxValue)
            {
                spookResource = spookOMeter.maxValue;
            }
        }

        //nav blocker size change based on scariness
        navBlocker.radius = (spookResource / 3) * scareRadiusMult;
        float circleVizSize = (spookResource / 3) * scareRadiusMult/10;
        debugCircleVisualizer1.transform.localScale =new Vector3(circleVizSize, circleVizSize, circleVizSize);

        spookOMeter.value = spookResource;
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        cameraBuddy.transform.position = Vector3.Lerp(cameraBuddy.transform.position, transform.position, camLerpSpeed);
        cameraBuddy.transform.rotation = Quaternion.Slerp(cameraBuddy.transform.rotation, cameraBuddyBuddy.transform.rotation, camLerpSpeed);
        cameraBuddyBuddy.transform.position = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "stealthzone")
        {
            stealthed = true;
        }
        if(other.tag == "mouseHole")
        {
            if(monsterType == 1)
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
