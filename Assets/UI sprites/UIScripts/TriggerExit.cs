using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExit : MonoBehaviour
{
    public GameObject canva1;
    public GameObject canva2;
    public Animator anim;
    public Animator anim2;

    public void PlayPressed()
    {
        anim.SetTrigger("Play");
    }

    public void BackPressed()
    {
        anim2.SetTrigger("Back");
    }

    public void menuTrigger1()
    {
        canva1.SetActive(true);
        canva2.SetActive(false);
    }public void menuTrigger2()
    {
        canva1.SetActive(false);
        canva2.SetActive(true);
        anim.SetTrigger("Entry");
    }
}
