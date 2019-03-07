using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlasher : MonoBehaviour
{
    [SerializeField] string text = "FLSHR_TXT";
    [SerializeField] float flashSpeed = 0.5f;
    float counter = 0;

    [Header("Flags")]
    [SerializeField] bool warning = false;
    [SerializeField] bool serious = false;

    [Header("References")]
    [SerializeField] Text idle;
    [SerializeField] Text warn;


    private void Start()
    {
        idle.enabled = true;
        idle.text = text;
        warn.enabled = false;
        warn.text = text;
    }

    private void Update()
    {
        if(!warning)
            warn.enabled = false;
        else
        {
            if (serious)
            {
                counter += Time.deltaTime;
                if(counter > flashSpeed)
                {
                    counter = 0;
                    warn.enabled = !warn.enabled;
                }
            }
            else
                warn.enabled = true;
        }
    }

    public void SetState(bool warning, bool serious)
    {
        this.warning = warning;
        this.serious = serious;
    }
}
