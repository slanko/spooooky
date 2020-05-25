using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallHideScript : MonoBehaviour
{
    MeshRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "wallDestroyer")
        {
            myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            foreach(Transform childTransform in transform)
            {
                MeshRenderer childMeshRenderer;
                if (childTransform.GetComponent<MeshRenderer>() != null)
                {
                    childMeshRenderer = childTransform.GetComponent<MeshRenderer>();
                    childMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "wallDestroyer")
        {
            myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            foreach (Transform childTransform in transform)
            {
                MeshRenderer childMeshRenderer;
                if (childTransform.GetComponent<MeshRenderer>() != null)
                {
                    childMeshRenderer = childTransform.GetComponent<MeshRenderer>();
                    childMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
        }
    }
}