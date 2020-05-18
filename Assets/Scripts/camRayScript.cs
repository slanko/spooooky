using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camRayScript : MonoBehaviour
{
    public GameObject buddyPlayer, wallHider;
    public LayerMask wallLayer;
    public RaycastHit camRayHit;
    public float rayCastWidthDistance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out camRayHit, Vector3.Distance(transform.position, buddyPlayer.transform.position), wallLayer))
        {
            Debug.DrawLine(transform.position, camRayHit.point, Color.green, 0.1f);
            wallHider.transform.position = camRayHit.point;
        }
        else
        {
            wallHider.transform.position = new Vector3(0, 50, 0);
        }
    }
}
