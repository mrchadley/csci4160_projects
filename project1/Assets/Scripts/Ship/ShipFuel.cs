using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFuel : MonoBehaviour
{
    [Header("Fuel Stats")]
    [Range(0.0f, 90.0f)] public float fuel = 90.0f;
    [SerializeField] float thrusterUsage = 1.0f;
    [SerializeField] float stabilizerUsage = 0.1f;
    public bool isThrusting = false;
    public bool isStabilizing = false;

    [Header("References")]
    [SerializeField] RectTransform gaugePointer;

    public void AdjustFuel(float amount)
    {
        fuel += amount;

        if (fuel >= 90.0f)
        {
            fuel = 90.0f;
        }
        if (fuel < 0.0f)
        {
            fuel = 0.0f;
        }

        gaugePointer.rotation = Quaternion.Euler(0, 0, -90.0f + fuel);
    }

    private void OnValidate()
    {
        gaugePointer.rotation = Quaternion.Euler(0, 0, -90.0f + fuel);
    }

    private void Update()
    {
        if(isThrusting)
        {
            AdjustFuel(-thrusterUsage * Time.deltaTime);
        }
        if (isStabilizing)
        {
            AdjustFuel(-stabilizerUsage * Time.deltaTime);
        }
    }


}
