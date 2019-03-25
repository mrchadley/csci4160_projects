using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToggler : MonoBehaviour
{
    [Tooltip("on in trigger, off out of trigger")]
    public GameObject[] toggleOn; // toggled on in OnTriggerEnter
    [Tooltip("off in trigger, on out of trigger")]
    public GameObject[] toggleOff; // toggled off in OnTriggerEnter
    [Tooltip("on in trigger, off out of trigger")]
    public Collider[] toggleOnTriggers; // toggled on in OnTriggerEnter

    public int triggerCounter = 0;
    bool isOn
    {
        get { return triggerCounter > 0; }
    }

    void SetOn(bool on)
    {
        foreach (GameObject tn in toggleOn)
            if(tn) tn.SetActive(on);
        foreach (GameObject tf in toggleOff)
            if(tf) tf.SetActive(!on);
        foreach (Collider tnt in toggleOnTriggers)
            if(tnt) tnt.enabled = on;
    }

    private void OnValidate()
    {
        SetOn(isOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            triggerCounter++;
            SetOn(isOn );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggerCounter--;
            SetOn(isOn);
        }
    }

}
