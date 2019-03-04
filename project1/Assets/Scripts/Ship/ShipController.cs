using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static ShipController instance;

    [Header("References")]
    [SerializeField] private Transform leftStabilizer;
    [SerializeField] private Transform rightStabilizer;

    [SerializeField] private Animator thrusterAnim;
    [SerializeField] private Animator leftStabAnim;
    [SerializeField] private Animator rightStabAnim;

    private Rigidbody2D rb;

    [Header("Thrust")]
    [SerializeField] private float mainThrustPower = 10.0f;
    [SerializeField] private float stabilizerPower = 2.0f;
    //create variables to make thrusting better

    ShipFuel shipFuel;
    ShipHealth shipHealth;

    CheckpointManager cm;

    bool countingDown = false;
    bool inEffector = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);
    }

    public void ShipReset()
    {
        rb.Sleep();

        if (cm.lastCheckpoint != null)
            transform.position = cm.lastCheckpoint.transform.position;
        else
            transform.position = Vector3.up * -4.85f;

        transform.rotation = Quaternion.identity;
        //TO-DO: create reset methods for these scripts
        shipFuel.AdjustFuel(90.0f);
        shipHealth.AdjustDamage(-100.0f);
        shipHealth.dead = false;

        Disable(2.0f);
    }

    void Disable(float time)
    {
        countingDown = true;
        StartCoroutine(Countdown(time));
        Debug.Log("disable");
    }

    IEnumerator Countdown(float time)
    {
        yield return new WaitForSeconds(time);
        countingDown = false;
    }

    void Start()
    {
        cm = CheckpointManager.instance;
        rb = GetComponent<Rigidbody2D>();
        shipFuel = GetComponent<ShipFuel>();
        shipHealth = GetComponent<ShipHealth>();
    }

    // TO-DO: Move physics stuff to fixed update
    void Update()
    {
        if(Input.GetButtonDown("Reset"))
        {
            ShipReset();            
        }

        if (shipFuel.fuel <= 0.0f || countingDown || shipHealth.dead)
        {
            shipFuel.isThrusting = false;
            shipFuel.isStabilizing = false;
            thrusterAnim.SetBool("IsThrusting", false);
            leftStabAnim.SetBool("IsThrusting", false);
            rightStabAnim.SetBool("IsThrusting", false);
            return;
        }

        float vert = Input.GetAxis("Vertical");
        if (vert < 0.0f) vert = 0.0f;

        rb.AddForce(transform.up * vert * mainThrustPower * Time.deltaTime * (inEffector ? 10.0f : 1.0f));
        thrusterAnim.SetBool("IsThrusting", (vert > 0.0f));
        shipFuel.isThrusting = (vert > 0.0f);

        float horiz = Input.GetAxis("Horizontal");
        Vector3 forceDir = ((horiz > 0) ? leftStabilizer.up : rightStabilizer.up) * -1.0f * Mathf.Abs(horiz);
        Vector3 forcePos = ((horiz > 0) ? leftStabilizer.position : rightStabilizer.position);

        rb.AddForceAtPosition(forceDir * stabilizerPower * Time.deltaTime * (inEffector ? 10.0f : 1.0f), forcePos);
        if(Mathf.Abs(horiz) > 0)
        {
            if(horiz > 0)
                leftStabAnim.SetBool("IsThrusting", true);
            else
                rightStabAnim.SetBool("IsThrusting", true);
            shipFuel.isStabilizing = true;
        }
        else
        {
            leftStabAnim.SetBool("IsThrusting", false);
            rightStabAnim.SetBool("IsThrusting", false);
            shipFuel.isStabilizing = false;
        }
    }
    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Effectors"));
        inEffector = false;
        foreach(Collider2D col in colliders)
        {
            inEffector |= col.usedByEffector;
        }
    }
}
