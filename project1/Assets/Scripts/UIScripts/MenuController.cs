using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public LowScoreObject[] lowScores = new LowScoreObject[5];


    public void ExitGame()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            UnityEditor.EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }

    public void SelectDifficulty(float difficultyMultiplier)
    {
        DifficultyManager.mult = difficultyMultiplier;

        SceneManager.LoadScene("Game");
    }

    public void LoadScores()
    {
        ScoreIO.Score[] scores = ScoreIO.instance.GetTopScores(lowScores.Length);
        for (int i = 0; i < lowScores.Length; i++)
        {
            if(i < scores.Length)
            {
                lowScores[i].SetText(scores[i].name, scores[i].score.ToString());
            }
            else
            {
                lowScores[i].SetText("N/A", "-");
            }
        }
    }
}
