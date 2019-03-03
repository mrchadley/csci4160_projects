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

    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if(collision.tag == "Player" && rb.velocity.sqrMagnitude < 0.0001f && rb.angularVelocity < 0.0001f)
        {
            if (manager.lastCheckpoint != this && manager.lastCheckpoint.index < index)
            {
                manager.lastCheckpoint = this;
                Debug.Log(name + " Reached!");
            }

            collision.SendMessage("AdjustDamage", -100.0f);
            collision.SendMessage("AdjustFuel", 90.0f);
        }
    }
}
