using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFuel : MonoBehaviour
{
    [Header("Fuel Stats")]
    [Range(0.0f, 90.0f)] public float fuel = 90.0f;
    public bool hasFuel
    {
        get { return fuel > 0.0f; }
    }

    [Header("References")]
    [SerializeField] RectTransform gaugePointer;
    [SerializeField] UIFlasher fuelFlash;

    public bool AdjustFuel(float amount)
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
        fuelFlash.SetState(fuel < 30.0f, fuel < 15.0f);

        return fuel > 0.0f;
    }

    private void OnValidate()
    {
        gaugePointer.rotation = Quaternion.Euler(0, 0, -90.0f + fuel);
    }
}
