using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallHideScript : MonoBehaviour
{
    camRayScript cameraRay;
    MeshRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        cameraRay = GameObject.Find("camBuddy/Main Camera").GetComponent<camRayScript>();
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "wallDestroyer")
        {
            myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "wallDestroyer")
        {
            myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}