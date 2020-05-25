using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pictureFrameScript : MonoBehaviour
{
    Rigidbody rb;
    public Material[] paintingTextures;
    public Renderer paintingRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnBecameVisible()
    {
        paintingRenderer.material = paintingTextures[Random.Range(0, paintingTextures.Length)];
    }
}
