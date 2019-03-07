using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] float impactDamage = 25.0f;

    float cos = Mathf.Cos(45.0f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 vel = collision.attachedRigidbody.velocity.normalized;
        if (Vector3.Dot(vel, transform.up) > 0)
        {
            StatCounter.instance.spikings++;
            collision.SendMessage("AdjustDamage", impactDamage);
        }
    }
}
