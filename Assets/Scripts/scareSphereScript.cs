using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scareSphereScript : MonoBehaviour
{
    public float stayTime, scarinessLevel;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("goByeBye", stayTime);
    }

    void goByeBye()
    {
        Destroy(gameObject);
    }
}
