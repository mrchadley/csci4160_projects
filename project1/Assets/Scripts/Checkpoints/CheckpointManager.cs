using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    [SerializeField] Checkpoint _lastCheckpoint;
    [Header("Checkpoint Text")]
    [SerializeField] float textTime = 3.0f;
    public Checkpoint lastCheckpoint
    {
        set
        {
            _lastCheckpoint = value;
            StopAllCoroutines();
            StartCoroutine("ShowText");
        }
        get
        {
            return _lastCheckpoint;
        }
    }
    [SerializeField] TextMeshProUGUI reachedText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);

        reachedText.enabled = false;
    }

    IEnumerator ShowText()
    {
        reachedText.color = Color.white;
        reachedText.enabled = true;
        yield return new WaitForSecondsRealtime(textTime);

        float t = 0;
        Color col = Color.white;
        
        while(col.a > 0)
        {
            col.a = Mathf.Lerp(1.0f, 0.0f, t);
            reachedText.color = col;
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        reachedText.enabled = false;

    }
}
