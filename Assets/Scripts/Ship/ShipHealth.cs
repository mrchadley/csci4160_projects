using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [Header("Damage Stats")]
    [Range(0.0f, 100.0f)] public float damage = 0.0f;
    [SerializeField] float impactDamageFactor = 1.0f;
    [SerializeField] float impactVelocityThreshold = 1.0f;
    [SerializeField] float frictionDamageFactor = 0.2f;
    [SerializeField] float frictionVelocityThreshold = 0.1f;

    [Header("References")]
    [SerializeField] RectTransform gaugePointer;
    [SerializeField] Collider2D body;
    [SerializeField] Collider2D foot;

    Rigidbody2D rb;
    float velocity = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AdjustDamage(float amount)
    {
        damage += amount;

        if(damage >= 100.0f)
        {
            Die();
            damage = 100.0f;
        }
        if(damage < 0.0f)
        {
            damage = 0.0f;
        }

        gaugePointer.rotation = Quaternion.Euler(0, 0, (90.0f - (damage / 100.0f * 90.0f)));
    }

    void Die()
    {
        Debug.Log("Died.");
    }

    private void OnValidate()
    {
        gaugePointer.rotation = Quaternion.Euler(0,0,(90.0f - (damage/100.0f * 90.0f)));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(velocity);
        if(velocity > impactVelocityThreshold)
        {
            AdjustDamage(impactDamageFactor * velocity);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(velocity > frictionVelocityThreshold && collision.otherCollider == body)
        {
            AdjustDamage(frictionDamageFactor * velocity * Time.fixedDeltaTime);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label((velocity > 0.001f ? velocity + "" : "0"));
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity.magnitude;
    }
}
