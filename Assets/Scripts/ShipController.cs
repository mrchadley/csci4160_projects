using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shipFuel = GetComponent<ShipFuel>();
        shipHealth = GetComponent<ShipHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Reset"))
        {
            rb.Sleep();
            transform.position = Vector3.up * -4.85f;
            transform.rotation = Quaternion.identity;
            shipFuel.AdjustFuel(90.0f);
            shipHealth.AdjustDamage(-100.0f);
        }

        if (shipFuel.fuel <= 0.0f)
        {
            shipFuel.isThrusting = false;
           shipFuel.isStabilizing = false;
            return;
        }

        float vert = Input.GetAxis("Vertical");
        if (vert < 0.0f) vert = 0.0f;

        rb.AddForce(transform.up * vert * mainThrustPower * Time.deltaTime);
        thrusterAnim.SetBool("IsThrusting", (vert > 0.0f));
        shipFuel.isThrusting = (vert > 0.0f);

        float horiz = Input.GetAxis("Horizontal");
        Vector3 forceDir = ((horiz > 0) ? leftStabilizer.up : rightStabilizer.up) * -1.0f * Mathf.Abs(horiz);
        Vector3 forcePos = ((horiz > 0) ? leftStabilizer.position : rightStabilizer.position);

        rb.AddForceAtPosition(forceDir * stabilizerPower * Time.deltaTime, forcePos);
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
}
