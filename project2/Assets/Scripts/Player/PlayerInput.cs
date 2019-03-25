using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{
    private CharacterController cc;

    [SerializeField] private float moveSpeed = 5.0f;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed;
        float vertical = Input.GetAxis("Vertical") * moveSpeed;

        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;

        cc.SimpleMove(moveDir);

    }        
}
