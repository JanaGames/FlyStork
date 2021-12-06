using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator FlyRunController;
    public Animator HeadController;
    public Animator TailController;

    public void Run() 
    {
        FlyRunController.SetTrigger("FlyRun");
        FlyRunController.SetTrigger("HeadRun");
        TailController.SetTrigger("TailRun");
    }
    public void Rigth() 
    {
        FlyRunController.SetTrigger("HeadRigth");
    }
    public void Left() 
    {
        FlyRunController.SetTrigger("HeadLeft");
    }
    public void Fly() 
    {
        TailController.SetTrigger("TailFly");
    }
    public void Shoot() 
    {
        FlyRunController.SetTrigger("Shoot");
    }
}
