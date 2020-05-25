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
    [Header("Inputs")]
    public KeyCode scareKey;
    public KeyCode useKey, upKey, downKey, leftKey, rightKey, camLeftKey, camRightKey, resetGameKey;
    public float moveSpeed;

    [Header("Spook Mechanics")]
    public bool stealthed;
    public Slider spookOMeter;
    public float spookResource, spookGainRate, spookDiminishRate;

    [Header("Camera Stuff")]
    public float camLerpSpeed;
    public GameObject cameraBuddy, cameraBuddyBuddy, debugCircleVisualizer1;

    [Header("AI Stuff")]
    public NavMeshObstacle navBlocker;

    [Header("Ability Stuff")]
    public slimeTrailAbility slimy;
    public KeyCode toggleSlime;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        slimy = GetComponent<slimeTrailAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        //big fat movement code block
        if (Input.GetKey(upKey))
        {
            transform.rotation = Quaternion.Euler(0, 0 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKey(downKey))
        {
            transform.rotation = Quaternion.Euler(0, 180 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKey(leftKey))
        {
            transform.rotation = Quaternion.Euler(0, -90 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKey(rightKey))
        {
            transform.rotation = Quaternion.Euler(0, 90 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKey(upKey) && Input.GetKey(rightKey))
        {
            transform.rotation = Quaternion.Euler(0, 45 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKey(upKey) && Input.GetKey(leftKey))
        {
            transform.rotation = Quaternion.Euler(0, -45 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKey(downKey) && Input.GetKey(rightKey))
        {
            transform.rotation = Quaternion.Euler(0, 135 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKey(downKey) && Input.GetKey(leftKey))
        {
            transform.rotation = Quaternion.Euler(0, 225 + cameraBuddy.transform.rotation.eulerAngles.y, 0);
        }
        //end big fat movement code block

        //lovely actions section
        if (Input.GetKeyDown(scareKey))
        {
            if(stealthed == false)
            {
                anim.SetTrigger("scare");
                aud.pitch = Random.Range(.5f, 1.5f);
                aud.Play();
            }
        }

        //debug slime trail enable/disable
        if (Input.GetKeyDown(toggleSlime))
        {
            if(slimy.weOutHereSliming == false)
            {
                slimy.weOutHereSliming = true;
            }
            else
            {
                slimy.weOutHereSliming = false;
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
        navBlocker.radius = (spookResource / 3) * 1.7f;
        float circleVizSize = (spookResource / 3) * 0.1674161f;
        debugCircleVisualizer1.transform.localScale =new Vector3(circleVizSize, circleVizSize, circleVizSize);

        spookOMeter.value = spookResource;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(upKey) || Input.GetKey(downKey) || Input.GetKey(leftKey) || Input.GetKey(rightKey))
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
            if (Input.GetKeyDown(useKey))
            {
                mouseHoleScript MSH;
                MSH = other.GetComponent<mouseHoleScript>();
                transform.position = new Vector3(MSH.otherMSH.transform.position.x, transform.position.y, MSH.otherMSH.transform.position.z);
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
