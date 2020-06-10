using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallHideScript : MonoBehaviour
{
    MeshRenderer myRenderer;
    Animator anim;

    private void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "wallDestroyer")
        {
            anim.SetBool("disappear", true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "wallDestroyer")
        {
            anim.SetBool("disappear", false);
        }
    }
}