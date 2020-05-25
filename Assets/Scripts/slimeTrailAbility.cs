using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeTrailAbility : MonoBehaviour
{
    public GameObject trailObject;
    public float dropRateMin, dropRateMax;
    public bool weOutHereSliming;
    bool slimed;
    // Start is called before the first frame update
    void Start()
    {
        if(weOutHereSliming == true)
        {
            dropSlime();
        }
    }

    void dropSlime()
    {
        if(weOutHereSliming == true)
    {
            if (Physics.Raycast(transform.position, Vector3.down, out var rayHit, 2))
            {
                Instantiate(trailObject, rayHit.point, transform.rotation);
                Invoke("dropSlime", Random.Range(dropRateMin, dropRateMax));
            }
        }

    }

    private void Update()
    {
        if(weOutHereSliming == true)
        {
            if(slimed == false)
            {
                dropSlime();
                slimed = true;
            }
        }
        else
        {
            slimed = false;
        }
    }
}
