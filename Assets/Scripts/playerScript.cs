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
    public GameObject monster1Model, monster1BabyModel, monster1FinalForm;
    public Sprite monster1Image1, monster1Image2, monster1Image3;
    public ParticleSystem monster1Particles;

    [Header("Ball Fella Values"), Tooltip("Values for the Funny Little Ball Fella. A fast little fella who isn't exactly scary but has SPEED abilities to back it up.")]
    public float monster2MoveSpeed;
    public float monster2ScareRadius, monster2BlockRadius, monster2SpookGain, monster2SpookDecay;
    public GameObject monster2Model, monster2BabyModel, monster2FinalForm;
    public Sprite monster2Image1, monster2Image2, monster2Image3;
    public ParticleSystem monster2Particles;

    [Header("Snake Fella Values"), Tooltip("Values for the Snake Fella. haha i stopped writing descriptions")]
    public float monster3MoveSpeed;
    public float monster3ScareRadius, monster3BlockRadius, monster3SpookGain, monster3SpookDecay;
    public GameObject monster3Model, monster3BabyModel, monster3FinalForm;
    public Sprite monster3Image1, monster3Image2, monster3Image3;
    public ParticleSystem monster3Particles;

    [Header("Corn Fella Values"), Tooltip("Values for the Corn Fella. ")]
    public float monster4MoveSpeed;
    public float monster4ScareRadius, monster4BlockRadius, monster4SpookGain, monster4SpookDecay;
    public GameObject monster4Model, monster4BabyModel, monster4FinalForm, monster4CornModeModel;
    public Sprite monster4Image1, monster4Image2, monster4Image3;
    public float oldSpookGain, cornModeSpookGain;
    public ParticleSystem monster4Particles;

    [Header("Global Inputs")]
    public KeyCode scareKey;
    public KeyCode useKey, camLeftKey, camRightKey, resetGameKey;
    public float moveSpeed;
    private float moveSpeedFactor; 

    [Header("Spook Mechanics")]
    public Animator playerIconAnim;
    public bool stealthed;
    public Slider spookOMeter;
    public float spookResource, spookGainRate, spookDiminishRate, spookDiminishRateSlow, spookDiminishRateFast;
    public SphereCollider scareRadius;
    public float scareRadiusMult;
    public bool babyMode;
    public Sprite spookLevelImage1, spookLevelImage2, spookLevelImage3;
    public Image monsterPortrait;
    public bool cornMode;
    public ParticleSystem darkParticles;



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

        aud.volume = PlayerPrefs.GetFloat("audioVolume");

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
            spookLevelImage1 = monster1Image1;
            spookLevelImage2 = monster1Image2;
            spookLevelImage3 = monster1Image3;
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
            spookLevelImage1 = monster2Image1;
            spookLevelImage2 = monster2Image2;
            spookLevelImage3 = monster2Image3;
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
            spookLevelImage1 = monster3Image1;
            spookLevelImage2 = monster3Image2;
            spookLevelImage3 = monster3Image3;
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
            spookLevelImage1 = monster4Image1;
            spookLevelImage2 = monster4Image2;
            spookLevelImage3 = monster4Image3;
            oldSpookGain = monster4SpookGain;
        }

        moveSpeedFactor = 1;
        anim2.speed = 3;

        spookDiminishRateFast = spookDiminishRate * 2.5f;
        spookDiminishRateSlow = spookDiminishRate;
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

        //update audio volume
        aud.volume = god.globalAudioVolume;

        //animator checks!!
        if (anim2.isActiveAndEnabled == false)
        {
            if (playerChoice.monsterSelection == playerScript.monsterType.WIGGLYFELLA)
            {
                if (spookResource > 1 && spookResource < 2)
                {
                    anim2 = monster1Model.GetComponent<Animator>();
                }
                if (spookResource > 2)
                {
                    anim2 = monster1FinalForm.GetComponent<Animator>();
                }
            }
            if (playerChoice.monsterSelection == playerScript.monsterType.BALLFELLA)
            {
                if (spookResource > 1 && spookResource < 2)
                {
                    anim2 = monster2Model.GetComponent<Animator>();
                }
                if (spookResource > 2)
                {
                    anim2 = monster2FinalForm.GetComponent<Animator>();
                }
            }
            if (playerChoice.monsterSelection == playerScript.monsterType.SNAKEFELLA)
            {
                if (spookResource > 1 && spookResource < 2)
                {
                    anim2 = monster3Model.GetComponent<Animator>();
                }
                if (spookResource > 2)
                {
                    anim2 = monster3FinalForm.GetComponent<Animator>();
                }
            }
            if (playerChoice.monsterSelection == playerScript.monsterType.CORNFELLA)
            {
                if (spookResource > 1 && spookResource < 2)
                {
                    anim2 = monster4Model.GetComponent<Animator>();
                }
                if (spookResource > 2)
                {
                    anim2 = monster4FinalForm.GetComponent<Animator>();
                }
            }
        }

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
                anim.SetTrigger("scare");
                anim2.SetTrigger("spook");
                aud.pitch = Random.Range(.5f, 1.5f);
                aud.Play();
        }

        //debug slime trail enable/disable
        if(playerChoice.monsterSelection == monsterType.SNAKEFELLA)
        {
            if (Input.GetKeyDown(useKey))
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

        if (playerChoice.monsterSelection == monsterType.CORNFELLA)
        {
            if (Input.GetKeyDown(useKey))
            {
                if (cornMode == false)
                {
                    cornMode = true;
                    spookGainRate = cornModeSpookGain;
                }
                else
                {
                    cornMode = false;
                    spookGainRate = oldSpookGain;
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
            if(darkParticles.isPlaying == true)
            {
                darkParticles.Stop();
            }
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
            if(darkParticles.isPlaying == false)
            {
                darkParticles.Play();
            }
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

        //baby mode bullshit

        if(spookResource < 1)
        {
            babyMode = true;
            monsterPortrait.sprite = spookLevelImage1;
        }
        else
        {
            babyMode = false;
            if(spookResource > 1 && spookResource < 2)
            {
                monsterPortrait.sprite = spookLevelImage2;
            }
            if(spookResource > 2)
            {
                monsterPortrait.sprite = spookLevelImage3;
            }
        } 
        if(cornMode == false)
        {
            if (babyMode == true)
            {
                if (playerChoice.monsterSelection == playerScript.monsterType.WIGGLYFELLA)
                {
                    monster1Model.SetActive(false);
                    monster1BabyModel.SetActive(true);
                }
                if (playerChoice.monsterSelection == playerScript.monsterType.BALLFELLA)
                {
                    monster2Model.SetActive(false);
                    monster2BabyModel.SetActive(true);
                }
                if (playerChoice.monsterSelection == playerScript.monsterType.SNAKEFELLA)
                {
                    monster3Model.SetActive(false);
                    monster3BabyModel.SetActive(true);
                }
                if (playerChoice.monsterSelection == playerScript.monsterType.CORNFELLA)
                {
                    monster4Model.SetActive(false);
                    monster4BabyModel.SetActive(true);
                    monster4CornModeModel.SetActive(false);
                }
            }
            else
            {
                if (playerChoice.monsterSelection == playerScript.monsterType.WIGGLYFELLA)
                {
                    if(spookResource > 1 && spookResource < 2)
                    {
                        monster1Model.SetActive(true);
                        monster1BabyModel.SetActive(false);
                        monster1FinalForm.SetActive(false);
                    }
                    if(spookResource > 2)
                    {
                        monster1FinalForm.SetActive(true);
                        monster1Model.SetActive(false);
                    }
                }
                if (playerChoice.monsterSelection == playerScript.monsterType.BALLFELLA)
                {
                    if (spookResource > 1 && spookResource < 2)
                    {
                        monster2Model.SetActive(true);
                        monster2BabyModel.SetActive(false);
                        monster2FinalForm.SetActive(false);
                    }
                    if (spookResource > 2)
                    {
                        monster2FinalForm.SetActive(true);
                        monster2Model.SetActive(false);
                    }
                }
                if (playerChoice.monsterSelection == playerScript.monsterType.SNAKEFELLA)
                {
                    if (spookResource > 1 && spookResource < 2)
                    {
                        monster3Model.SetActive(true);
                        monster3BabyModel.SetActive(false);
                        monster3FinalForm.SetActive(false);
                    }
                    if (spookResource > 2)
                    {
                        monster3FinalForm.SetActive(true);
                        monster3Model.SetActive(false);
                    }
                }
                if (playerChoice.monsterSelection == playerScript.monsterType.CORNFELLA)
                {
                    if (spookResource > 1 && spookResource < 2)
                    {
                        monster4Model.SetActive(true);
                        monster4BabyModel.SetActive(false);
                        monster4CornModeModel.SetActive(false);
                        monster4FinalForm.SetActive(false);
                    }
                    if (spookResource > 2)
                    {
                        monster4FinalForm.SetActive(true);
                        monster4Model.SetActive(false);
                        monster4CornModeModel.SetActive(false);
                    }
                }
            }
        }
        if(cornMode == true)
        {
            monster4Model.SetActive(false);
            monster4BabyModel.SetActive(false);
            monster4FinalForm.SetActive(false);
            monster4CornModeModel.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            if(cornMode == false)
            {
                transform.Translate(Vector3.forward * (moveSpeed * moveSpeedFactor) * Time.deltaTime);
            }
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

        if(other.tag == "scareDecreaseZone")
        {
            spookDiminishRate = spookDiminishRateFast;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "stealthzone")
        {
            stealthed = false;
        }
        if (other.tag == "scareDecreaseZone")
        {
            spookDiminishRate = spookDiminishRateSlow;
        }
    }
}
