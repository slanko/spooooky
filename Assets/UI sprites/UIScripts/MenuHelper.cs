using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHelper : MonoBehaviour
{
    public TriggerExit Exit;

   public void transitionOut(string levelToTransitionTo)
    {
        Exit.menuTrigger(levelToTransitionTo);
    }

}
