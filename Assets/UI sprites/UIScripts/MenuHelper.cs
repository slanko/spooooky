using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHelper : MonoBehaviour
{
    public TriggerExit Exit;



   public void Canvas1()
    {
        Exit.menuTrigger1();
       
    }

    public void Canvas2()
    {
        Exit.menuTrigger2();
    }

    public void Canvas3()
    {
        Exit.menuTrigger3();
    }

}
