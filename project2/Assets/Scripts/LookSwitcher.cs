using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookSwitcher : MonoBehaviour
{
    public GameObject lookAwayPoint;

    [Tooltip("switched on when looking away")]
    public GameObject[] switchOn; //switched on OnTriggerEnter
    [Tooltip("switched off when looking away")]
    public GameObject[] switchOff; //switched off OnTriggerEnter


    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        Transform playerTrans = other.transform;

        Vector2 pFwd = new Vector2(playerTrans.forward.x, playerTrans.forward.z);
        Vector3 laDir = lookAwayPoint.transform.position - playerTrans.position;
        Vector2 laDir2D = new Vector2(laDir.x, laDir.z);

        float angle = Vector2.Angle(pFwd, laDir2D);

        if (angle > 135)
        {
            foreach (GameObject s in switchOn)
                if(s) s.SetActive(true);
            foreach (GameObject s in switchOff)
                if(s) s.SetActive(false);
        }
    }
}
