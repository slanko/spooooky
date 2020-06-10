using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furnitureBonkScript : MonoBehaviour
{
    public GameObject sphereOfFear;
    public float collisionMagnitude, sizeMultiplier, scareMultiplier;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "furniture")
        {
            ContactPoint contact = collision.contacts[0];
            if (collision.relativeVelocity.magnitude > collisionMagnitude)
            {
                GameObject lastSphere = Instantiate(sphereOfFear, contact.point, new Quaternion(0, 0, 0, 0));
                scareSphereScript scareSphere;
                float scalebuddy = collision.relativeVelocity.magnitude * sizeMultiplier;
                lastSphere.transform.localScale = new Vector3(scalebuddy, scalebuddy, scalebuddy);
                scareSphere = lastSphere.GetComponent<scareSphereScript>();
                scareSphere.scarinessLevel = collision.relativeVelocity.magnitude * scareMultiplier; 
            }
        }

    }
}
