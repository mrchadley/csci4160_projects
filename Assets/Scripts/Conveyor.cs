using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] bool isRightward = false;

    private void Start()
    {
        Animator[] anims = GetComponentsInChildren<Animator>();

        foreach(Animator anim in anims)
        {
            anim.SetBool("right", isRightward);
        }
    }
}
