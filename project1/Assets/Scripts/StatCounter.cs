using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatCounter : MonoBehaviour
{
    static StatCounter instance;

    public float fuelBurned = 0.0f;
    public float damageTaken = 0.0f;
    public int zaps = 0;
    public int resets = 0; //deaths + strandings
    public int collisions = 0;
    public float distanceDragged = 0.0f; //distance travelled while colliding w/ non-conveyor
    public float distanceConveyed = 0.0f; //distance travelled on conveyors
    public float time = 0.0f;


    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);
    }
}
