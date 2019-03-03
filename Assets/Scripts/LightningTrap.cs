using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrap : MonoBehaviour
{
    [SerializeField] GameObject lightning;
    [SerializeField] float onTime = 2.0f; 
    [SerializeField] float offTime = 5.0f;
    [SerializeField] float effectTime = 3.0f;

    bool trapEnabled = false;

    float countdown = 0.0f;

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
        }
        lightning.SetActive(trapEnabled);
    }

    private void OnTriggerEnter(Collider2D collision)
    {
        if(trapEnabled) collision.SendMessage("Disable", effectTime);
    }
}
