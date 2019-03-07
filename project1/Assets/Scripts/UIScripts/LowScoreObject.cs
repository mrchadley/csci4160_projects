using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LowScoreObject : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public void SetText(string label, string score)
    {
        nameText.text = label;
        scoreText.text = score;
    }
}
