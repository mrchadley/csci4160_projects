using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrap : MonoBehaviour
{
    [SerializeField] GameObject lightning;
    [SerializeField] float timeOffset = 0.0f;
    [SerializeField] float onTime = 2.0f; 
    [SerializeField] float offTime = 5.0f;
    [SerializeField] float effectTime = 3.0f;

    bool trapEnabled = false;
    bool zapped = false;

    float countdown = 0.0f;


    private void Start()
    {
        countdown = timeOffset;
    }

    private void Update()
    {
        countdown += Time.deltaTime;

        if (countdown < onTime)
        {
            trapEnabled = true;
        }
        else if (countdown < (onTime + offTime))
        {
            trapEnabled = false;
        }
        else
        {
            countdown = 0.0f;
            zapped = false;
        }
        lightning.SetActive(trapEnabled);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (trapEnabled && !zapped)
        {
            collision.SendMessage("Disable", effectTime);
            zapped = true;
        }
    }
}
