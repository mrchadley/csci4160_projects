using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitcher : MonoBehaviour
{
    [Tooltip("switched on in OnTriggerEnter")]
    public GameObject[] switchOn; //switched on OnTriggerEnter
    [Tooltip("switched off in OnTriggerEnter")]
    public GameObject[] switchOff; //switched off OnTriggerEnter

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach (GameObject s in switchOn)
                if(s) s.SetActive(true);
            foreach (GameObject s in switchOff)
                if(s)s.SetActive(false);
        }
    }
}
