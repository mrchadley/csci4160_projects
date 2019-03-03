using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] bool isRightward = false;

    SurfaceEffector2D surface;

    private void Start()
    {
        surface = GetComponent<SurfaceEffector2D>();

        surface.speed = (isRightward ? 1.2f : -1.2f);

        Animator[] anims = GetComponentsInChildren<Animator>();

        foreach(Animator anim in anims)
        {
            anim.SetBool("right", isRightward);
        }
    }
}
