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

    [Header("UI Stuff")]
    [SerializeField] TextMeshProUGUI pausedText;
    [SerializeField] TextMeshProUGUI goText;
    [SerializeField] float textTime = 3.0f;


    private Rigidbody2D rb;

    [Header("Thrust")]
    [SerializeField] [Range(0.0f, 1.0f)]
    float zapThrustChance = 0.2f;

    ShipFuel fuel;
    ShipHealth health;

    CheckpointManager cm;
    StatCounter sc;

    bool paused = false;
    bool zapped = false;
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
        fuel.stranded = false;
        health.AdjustDamage(-100.0f);
        health.dead = false;

        controlsEnabled = false;
        StartCoroutine(Resetting(2.0f));
    }
    void Zap(float time)
    {
        sc.zaps++;
        float rand = Random.Range(-1.0f, 1.0f);
        bool thrust = (rand < zapThrustChance && rand > -zapThrustChance);

        zapped = true;
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
        zapped = false;
        sysFlash.SetState(false, false);
    }


    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);

        Debug.Log("Difficulty Multiplier: " + DifficultyManager.mult);
    }
    private void Start()
    {
        cm = CheckpointManager.instance;
        sc = StatCounter.instance;
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector2.zero;
        fuel = GetComponent<ShipFuel>();
        health = GetComponent<ShipHealth>();

        goText.enabled = false;
        paused = false;
        pausedText.gameObject.SetActive(paused);

        StartCoroutine(ShowText());
    }
    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
            Time.timeScale = (paused ? 0.0f : 1.0f);
            pausedText.gameObject.SetActive(paused);
        }

        if ((controlsEnabled || zapped) && !fading) sc.time += Time.deltaTime;

        if (Input.GetButtonDown("Reset") && !fading)
        {
            ShipReset();
            sc.resets++;
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
        if (fading)
        {
            sc.CalculateScore();
            Destroy(gameObject, 2.0f);
        }
    }

    bool refueling = false;
    void RefuelRepair()
    {
        if (!refueling && (fuel.fuel < 85.0f || health.damage > 5.0f))
        {
            sc.refuels++;
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
        yield return new WaitForSecondsRealtime(textTime);

        float t = 0;
        Color col = Color.white;

        while (col.a > 0)
        {
            col.a = Mathf.Lerp(1.0f, 0.0f, t);
            goText.color = col;
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        goText.enabled = false;

    }
}
