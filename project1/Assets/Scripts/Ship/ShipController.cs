using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipController : MonoBehaviour
{
    public static ShipController instance;

    [Header("References")]
    [SerializeField] Thruster main;
    [SerializeField] Thruster left;
    [SerializeField] Thruster right;
    [SerializeField] UIFlasher sysFlash;

    [Header("Go Text")]
    [SerializeField] TextMeshProUGUI goText;
    [SerializeField] float textTime = 3.0f;
    private Rigidbody2D rb;

    [Header("Thrust")]
    [SerializeField] [Range(0.0f, 1.0f)]
    float zapThrustChance = 0.2f;

    ShipFuel fuel;
    ShipHealth health;

    CheckpointManager cm;

    bool _controlsEnabled = true;
    public bool controlsEnabled
    {
        set
        {
            _controlsEnabled = value;
            if (_controlsEnabled)
            {
                left.isThrusting = Input.GetButton("ThrusterLeft");
                right.isThrusting = Input.GetButton("ThrusterRight");
                main.isThrusting = Input.GetButton("ThrusterMain");

                bool turbo = Input.GetButton("Turbo");
                left.isTurbo = turbo;
                right.isTurbo = turbo;
            }
            else
            {
                main.isThrusting = false;
                left.isThrusting = false;
                right.isThrusting = false;

                left.isTurbo = false;
                right.isTurbo = false;
            }
        }
        get
        {
            return _controlsEnabled;
        }
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
        fuel.AdjustFuel(90.0f);
        health.AdjustDamage(-100.0f);
        health.dead = false;

        controlsEnabled = false;
        StartCoroutine(Resetting(2.0f));
    }
    void Zap(float time)
    {
        float rand = Random.Range(-1.0f, 1.0f);
        bool thrust = (rand < zapThrustChance && rand > -zapThrustChance);

        controlsEnabled = false;
        sysFlash.SetState(true, thrust);
        left.isThrusting = (thrust && rand < 0);
        left.isTurbo = thrust;
        right.isThrusting = (thrust && rand > 0);
        right.isTurbo = thrust;

        StartCoroutine(Resetting(time));
    }
    IEnumerator Resetting(float time)
    {
        yield return new WaitForSeconds(time);

        controlsEnabled = true;
        sysFlash.SetState(false, false);
    }


    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);
    }
    private void Start()
    {
        cm = CheckpointManager.instance;
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector2.zero;
        fuel = GetComponent<ShipFuel>();
        health = GetComponent<ShipHealth>();

        goText.enabled = false;
    }
    private void Update()
    {
        /*
        float vert;

        if (!faded)
        {
            if (Input.GetButtonDown("Reset"))
            {
                ShipReset();
            }

            if ((shipFuel.fuel <= 0.0f || countingDown || shipHealth.dead))
            {
                shipFuel.isThrusting = false;
                shipFuel.isStabilizing = false;
                thrusterAnim.SetBool("IsThrusting", false);
                leftStabAnim.SetBool("IsThrusting", false);
                rightStabAnim.SetBool("IsThrusting", false);
                return;
            }

            float horiz = Input.GetAxis("Horizontal");
            Vector3 forceDir = ((horiz > 0) ? leftStabilizer.up : rightStabilizer.up) * -1.0f * Mathf.Abs(horiz);
            Vector3 forcePos = ((horiz > 0) ? leftStabilizer.position : rightStabilizer.position);

            rb.AddForceAtPosition(forceDir * stabilizerPower * Time.deltaTime * (Input.GetButton("Turbo") ? turboMultiplier : 1.0f) * (inEffector ? 10.0f : 1.0f), forcePos);
            shipFuel.isTurbo = Input.GetButton("Turbo");
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
            vert = Input.GetAxis("Vertical");
            if (vert < 0.0f) vert = 0.0f;
            shipFuel.isThrusting = (vert > 0.0f);
            rb.AddForce(transform.up * vert * mainThrustPower * Time.deltaTime * (inEffector ? 10.0f : 1.0f));
            thrusterAnim.SetBool("IsThrusting", (vert > 0.0f));

        }
        else
        {
            rb.AddForce(transform.up * mainThrustPower * Time.deltaTime);
            
        }
        */


        if (Input.GetButtonDown("Reset"))
        {
            ShipReset();
        }

        if (controlsEnabled)
        {
            if (Input.GetButtonDown("ThrusterMain"))
                main.isThrusting = true;
            if (Input.GetButtonUp("ThrusterMain"))
                main.isThrusting = false;

            if (Input.GetButtonDown("ThrusterLeft") && !right.isThrusting)
                left.isThrusting = true;
            if (Input.GetButtonUp("ThrusterLeft"))
            {
                left.isThrusting = false;
                right.isThrusting = Input.GetButton("ThrusterRight");
            }
            

            if (Input.GetButtonDown("ThrusterRight") && !left.isThrusting)
                right.isThrusting = true;
            if (Input.GetButtonUp("ThrusterRight"))
            {
                right.isThrusting = false;
                left.isThrusting = Input.GetButton("ThrusterLeft");
            }

            if (Input.GetButtonDown("Turbo"))
            {
                left.isTurbo = true;
                right.isTurbo = true;
            }
            if (Input.GetButtonUp("Turbo"))
            {
                right.isTurbo = false;
                left.isTurbo = false;
            }
        }
    }

    public bool fading = false;
    private void Fade()
    {
        fading = true;
        controlsEnabled = false;
        main.isThrusting = true;

        rb.freezeRotation = true;
        transform.rotation = Quaternion.identity;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Out of view!");
        if(fading) Destroy(gameObject, 2.0f);
    }

    bool refueling = false;
    void RefuelRepair()
    {

        if (!refueling && (fuel.fuel < 90.0f || health.damage > 0.0f))
        {
            refueling = true;
            controlsEnabled = false;
            StartCoroutine(RefuelingRepairing());
        }
    }
    IEnumerator RefuelingRepairing()
    {
        while(fuel.fuel < 90.0f || health.damage > 0.0f)
        {
            fuel.AdjustFuel(9.0f * Time.deltaTime);
            health.AdjustDamage(-20.0f * Time.deltaTime);
            yield return null;
        }
        controlsEnabled = true;
        refueling = false;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        goText.color = Color.white;
        goText.enabled = true;
        yield return new WaitForSeconds(textTime);

        float t = 0;
        Color col = Color.white;

        while (col.a > 0)
        {
            col.a = Mathf.Lerp(1.0f, 0.0f, t);
            goText.color = col;
            t += Time.deltaTime;
            yield return null;
        }

        goText.enabled = false;

    }
}
