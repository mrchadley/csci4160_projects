using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100;

    public Slider healthBar;

    private void Start()
    {
        healthBar.value = Mathf.Max(0f, health / 100.0f);
    }

    void ApplyDamage(int amount)
    {
        health -= amount;

        healthBar.value = Mathf.Max(0f, health / 100.0f);

        if(health <= 0)
        {
            SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            healthBar.gameObject.SetActive(false);
        }
    }
}
