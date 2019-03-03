using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] float impactDamage = 25.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("AdjustDamage", impactDamage);
    }
}
