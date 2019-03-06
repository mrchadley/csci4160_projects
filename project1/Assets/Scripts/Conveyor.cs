using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] float speed = 1.2f;
    [SerializeField] bool isRightward = false;

    SurfaceEffector2D surface;

    private void Start()
    {
        surface = GetComponent<SurfaceEffector2D>();

        speed = Mathf.Abs(speed);

        surface.speed = (isRightward ? speed : -speed);

        Animator[] anims = GetComponentsInChildren<Animator>();

        foreach(Animator anim in anims)
        {
            anim.SetBool("right", isRightward);
        }
    }
}
