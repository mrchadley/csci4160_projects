using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint lastCheckpoint;

    public static CheckpointManager instance;

    private void Awake()
    {
        if (CheckpointManager.instance == null) CheckpointManager.instance = this;
        else DestroyImmediate(this);
    }
}
