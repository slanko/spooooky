using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camBuddyScript : MonoBehaviour
{
    public float minSize, maxSize, moveSpeed;
    public KeyCode moveInKey, moveOutKey;
    Camera meCam;
    // Start is called before the first frame update
    void Start()
    {
        meCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(moveInKey))
        {
            if(meCam.orthographicSize > minSize)
            {
                meCam.orthographicSize = meCam.orthographicSize - (moveSpeed * Time.deltaTime);
            }

        }
        if (Input.GetKey(moveOutKey))
        {
            if(meCam.orthographicSize < maxSize)
            {
                meCam.orthographicSize = meCam.orthographicSize + (moveSpeed * Time.deltaTime);
            }

        }
    }
}
