using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.002f;
    [SerializeField] private float dampingFactor = 0.05f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    private float xSmoothed = 0.0f;
    private float ySmoothed = 0.0f;

    private float xVel = 0.0f;
    private float yVel = 0.0f;



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        yRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        xSmoothed = Mathf.SmoothDamp(xSmoothed, xRotation, ref xVel, dampingFactor);
        ySmoothed = Mathf.SmoothDamp(ySmoothed, yRotation, ref yVel, dampingFactor);

        transform.localRotation = Quaternion.Euler(xSmoothed, 0, 0);
        playerBody.rotation = Quaternion.Euler(0, ySmoothed, 0);
    }
}
