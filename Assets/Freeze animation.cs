using UnityEngine;
using IST360Tools;

public class Freezeanimation : CustomActionScript
{
    //public AnimationClip anim;
    public Animator anim;


    public override void DoAction()
    {
        anim.speed = 0;
    }
}