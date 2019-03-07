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
        if (ShipController.instance.fading) return true;

        if (amount < 0)
        {
            StatCounter.instance.fuelBurned -= amount;
            amount *= DifficultyManager.mult;
        }
        fuel += amount;

        if (fuel >= 90.0f)
        {
            fuel = 90.0f;
        }
        if (fuel < 0.0f)
        {
            fuel = 0.0f;
            if(!stranded)
            {
                Stranded();
            }
        }

        gaugePointer.rotation = Quaternion.Euler(0, 0, -90.0f + fuel);
        fuelFlash.SetState(fuel < 30.0f, fuel < 15.0f);

        return fuel > 0.0f;
    }

    public bool stranded = false;
    void Stranded()
    {
        Debug.Log("Died.");
        stranded = true;
        StatCounter.instance.strandings++;
        StartCoroutine(WaitAndReset());
    }

    private void OnValidate()
    {
        gaugePointer.rotation = Quaternion.Euler(0, 0, -90.0f + fuel);
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(3.0f);
        ShipController.instance.ShipReset();
    }
}
