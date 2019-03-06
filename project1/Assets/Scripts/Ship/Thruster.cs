using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] ShipFuel fuel;

    [Header("Thruster Variables")]
    [SerializeField] float thrustPower = 10.0f; //main 250, side = 50
    [SerializeField] float fuelUsage = 1.0f; //main = 2, side = 0.5
    [SerializeField] float turboMultiplier = 1.0f; //side = 4, main = 1

    bool _isThrusting = false;
    public bool isThrusting
    {
        set
        {
            _isThrusting = value;
            anim.SetBool("IsThrusting", _isThrusting && (fuel.hasFuel || ShipController.instance.fading));
        }
        get
        {
            return _isThrusting;
        }
    }

    bool _isTurbo = false;
    public bool isTurbo
    {
        set
        {
            _isTurbo = value;
            anim.SetBool("IsTurbo", _isTurbo && fuel.hasFuel);
        }
        get
        {
            return _isTurbo;
        }
    }

    private void Update()
    {
        if (isThrusting)
        {
            if(!fuel.AdjustFuel(-fuelUsage * Time.deltaTime * (_isTurbo ? turboMultiplier : 1.0f)))
            {
                if (!ShipController.instance.fading)
                {
                    anim.SetBool("IsThrusting", false);
                    anim.SetBool("IsTurbo", false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(_isThrusting && (fuel.hasFuel || ShipController.instance.fading))
            rb.AddForceAtPosition(transform.up * thrustPower * Time.fixedDeltaTime * (_isTurbo ? turboMultiplier : 1.0f), transform.position);
    }
}
