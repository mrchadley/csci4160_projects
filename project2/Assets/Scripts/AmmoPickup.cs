using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    int rounds = 10;
    void Start()
    {
        rounds = Random.Range(10, 21);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.BroadcastMessage("PickupAmmo", rounds);
            Destroy(gameObject);
        }
    }
}
