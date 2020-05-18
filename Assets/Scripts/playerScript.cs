using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{
    public KeyCode upKey, downKey, leftKey, rightKey, camLeftKey, camRightKey, resetGameKey;
    public bool stealthed;
    public Slider spookOMeter;
    public float spookResource, spookGainRate, spookDiminishRate;
    public GameObject cameraBuddy, cameraBuddyBuddy;
    public float moveSpeed, camLerpSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
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

        //camera controls part, this changes the cameraBuddy's Buddy's position so the camera buddy can lerp to it's rotation (it's an easier but slightly messy way to doing it)
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
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "stealthzone")
        {
            stealthed = false;
        }
    }
}
