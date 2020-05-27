using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstPersonCamScript : MonoBehaviour
{
    public GameObject buddy;
    public float camLerpSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = buddy.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, buddy.transform.rotation, camLerpSpeed);
    }
}
