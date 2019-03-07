using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFader : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] SpriteRenderer sprite;

    [SerializeField] float fadeStart = 300.0f;
    [SerializeField] float fadeLength = 150.0f;

    [SerializeField] Color col;

    bool faded = false;


    void Update()
    {
        float t = (player.position.y - fadeStart) / fadeLength;

        col = Color.white;
        col.a = Mathf.Lerp(0.0f, 1.0f, t);


        sprite.color = col;

        if (t >= 1)
        {
            player.SendMessage("Fade");
            enabled = false;
        }
    }
}
