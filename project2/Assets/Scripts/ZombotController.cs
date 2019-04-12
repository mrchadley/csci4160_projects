using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ZombotController : MonoBehaviour
{
    public Transform playerPos;
    NavMeshAgent nav;
    Animator anim;
    public float navTargetRefreshDelay = 0.1f;

    bool playerSpotted = false;
    float spotDelay = 0;

    public float attackDistance = 1.5f;
    public int attackDamage = 10;
    public float attackDelay = 0.5f;
    float distToPlayer = 1000000.0f;

    float attackCounter = 0;

    public GameObject ammoPickup;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        attackCounter = attackDelay;

        nav.avoidancePriority = 10;

        StartCoroutine(NavTargetUpdate());
    }

    void Update()
    {
        if (spotDelay > 0) spotDelay -= Time.deltaTime;
        if (attackCounter > 0) attackCounter -= Time.deltaTime;

        anim.SetFloat("Speed", nav.velocity.magnitude / nav.speed);
        distToPlayer = Vector3.Distance(playerPos.position, transform.position);
        if (attackCounter <= 0 && distToPlayer <= attackDistance)
        {
            anim.SetTrigger("Attack");
            nav.isStopped = true;
        }

        
    }


    IEnumerator NavTargetUpdate()
    {
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
            NavMeshPath path = new NavMeshPath();
            nav.CalculatePath(playerPos.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                if (!playerSpotted)
                {
                    yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
                    anim.SetTrigger("PlayerSpotted");
                    playerSpotted = true;
                    spotDelay = 2.8f;
                }
                if(spotDelay <= 0)
                {
                    nav.SetPath(path);
                }
            }

            yield return new WaitForSeconds(navTargetRefreshDelay);
        }
    }

    public void Attack()
    {
        Debug.Log(name + " attacking");
        if(distToPlayer <= attackDistance)
        {
            playerPos.BroadcastMessage("ApplyDamage", attackDamage);
        }
    }
    public void ResetNav()
    {
        if (nav.enabled)
        {
            nav.isStopped = false;
            anim.ResetTrigger("Attack");
            attackCounter = attackDelay;
        }
    }

    void Die()
    {
        StopAllCoroutines();
        anim.SetTrigger("Dead");

        Instantiate(ammoPickup, transform.position + Vector3.up, transform.rotation);
        
        nav.enabled = false;
        enabled = false;

    }
}
