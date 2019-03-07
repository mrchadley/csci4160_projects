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
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] UIFlasher dmgFlash;
    [SerializeField] ParticleSystem sparksPrefab;
    [SerializeField] ParticleSystem dragSparksPrefab;

    Rigidbody2D rb;
    StatCounter sc;

    float vMag = 0.0f;
    Vector3 velocity = Vector3.zero;

    public bool dead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sc = StatCounter.instance;
    }

    public void AdjustDamage(float amount)
    {
        if (amount > 0)
        {
            sc.damageTaken += amount;
            amount *= DifficultyManager.mult;
        }

        damage += amount;

        if (damage >= 100.0f)
        {
            if(!dead) Die();
            damage = 100.0f;
        }
        if (damage < 0.0f)
        {
            damage = 0.0f;
        }

        gaugePointer.rotation = Quaternion.Euler(0, 0, (90.0f - (damage / 100.0f * 90.0f)));
        dmgFlash.SetState(damage > 70.0f, damage > 85.0f);
    }

    void Die()
    {
        Debug.Log("Died.");
        dead = true;
        sc.deaths++;
        ShipController.instance.controlsEnabled = false;
        if(explosionPrefab != null) StartCoroutine(ExplodeAndWait());
    }

    private void OnValidate()
    {
        gaugePointer.rotation = Quaternion.Euler(0, 0, (90.0f - (damage / 100.0f * 90.0f)));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 norm = collision.GetContact(0).normal;
        float vNorm = Vector3.Project(velocity, new Vector3(norm.x, norm.y)).magnitude;
        if (vNorm > impactVelocityThreshold)
        {
            sc.collisions++;
            AdjustDamage(impactDamageFactor * vNorm);

            Instantiate(sparksPrefab, collision.GetContact(0).point, transform.rotation);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (vMag > frictionVelocityThreshold && collision.otherCollider == body && !collision.collider.usedByEffector)
        {
            AdjustDamage(frictionDamageFactor * vMag * Time.fixedDeltaTime);
            sc.distanceDragged += vMag * Time.fixedDeltaTime;
            Instantiate(dragSparksPrefab, collision.GetContact(0).point, transform.rotation);
        }
        else
        {
            if (collision.otherCollider == body && collision.collider.usedByEffector)
            {
                sc.distanceConveyed += vMag * Time.fixedDeltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        vMag = velocity.magnitude;
    }

    IEnumerator ExplodeAndWait()
    {
        GameObject expInstance = Instantiate(explosionPrefab, transform);
        float destroyTime = expInstance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(expInstance, destroyTime);
        yield return new WaitForSeconds(3.0f);
        ShipController.instance.ShipReset();
    }
}