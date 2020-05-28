using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedAbilityScript : MonoBehaviour
{

    playerScript pS;
    public float speedyBoost, speedyTimeMax;
    private float speedyTime;
    public TrailRenderer speedTrail;
    // Start is called before the first frame update
    void Start()
    {
        pS = GetComponent<playerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (speedyTime > 0) 
        {
            speedyTime = Mathf.Max(0, speedyTime - Time.deltaTime);
            speedTrail.emitting = true;
            if(speedyTime == 0)
            {
                pS.setSpeedFactor(1);
                speedTrail.emitting = false;
            }
        }
        

        if (playerChoice.monsterSelection == playerScript.monsterType.BALLFELLA && speedyTime == 0)
        {
            if(Input.GetKeyDown(pS.useKey) || Input.GetButtonDown("Interact"))
            {
                speedyTime = speedyTimeMax;
                pS.setSpeedFactor(speedyBoost);

            }
        }
    }

    IEnumerator speedyBoostCoroutine()
    {
        int i = 0;
        if(i < speedyTime)
        {

            i++;
            yield return new WaitForEndOfFrame();
        }
    }
}
