using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager manager;
    public int index = 0;

    private void Start()
    {
        manager = CheckpointManager.instance;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.attachedRigidbody;
        if(rb.tag == "Player" && rb.velocity.sqrMagnitude < 0.0001f && rb.angularVelocity < 0.0001f && Mathf.Abs(Vector3.Angle(rb.transform.up, Vector3.up)) < 0.1f)
        {
            if (manager.lastCheckpoint != this && manager.lastCheckpoint.index < index)
            {
                manager.lastCheckpoint = this;
                Debug.Log(name + " Reached!");
            }

            rb.SendMessage("RefuelRepair");
        }
    }
}
