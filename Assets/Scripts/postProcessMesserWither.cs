using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class postProcessMesserWither : MonoBehaviour
{
    godScript god;
    playerScript pS;
    Vignette vig;
    Animator anim;

    [Tooltip("Values for the different states of the stealth system. Normal default is 0.2, values from 0 to 1")]
    public float vignetteValue;

    [Tooltip("Object with the global Post Processing Volume on it")]
    public PostProcessVolume postProcessObject;
    
    // Start is called before the first frame update
    void Start()
    {
        god = GameObject.Find("GOD").GetComponent<godScript>();
        pS = GameObject.Find("Player").GetComponent<playerScript>();
        anim = GetComponent<Animator>();
        postProcessObject.profile.TryGetSettings(out vig);
    }

    // Update is called once per frame
    void Update()
    {
        vig.smoothness.value = vignetteValue;

        if(pS.stealthed == true)
        {
            anim.SetBool("stealthed", true);
        }
        else
        {

            anim.SetBool("stealthed", false);
        }

    }
}
