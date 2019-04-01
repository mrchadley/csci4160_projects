using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLift : MonoBehaviour
{
    public Animator anim;

    public bool lifted = false;

    private void Start()
    {
        anim.SetBool("IsUp", lifted);
    }

    public void Toggle()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("BoxLifting") || info.IsName("BoxLowering")) return;

        lifted = !lifted;
        anim.SetBool("IsUp", lifted);
    }
}
