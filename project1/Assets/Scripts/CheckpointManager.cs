using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint lastCheckpoint;

    public static CheckpointManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);
    }
}
