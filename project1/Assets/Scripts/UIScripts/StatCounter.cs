using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StatCounter : MonoBehaviour
{
    public static StatCounter instance;

    public int zaps = 0;
    public int resets = 0;
    public int deaths = 0;
    public int refuels = 0;
    public int spikings = 0;
    public int strandings = 0;
    public int collisions = 0;
    public float fuelBurned = 0.0f;
    public float damageTaken = 0.0f;
    public float distanceDragged = 0.0f;    //distance travelled while colliding w/ non-conveyor
    public float distanceConveyed = 0.0f;   //distance travelled on conveyors
    public float time = 0.0f;               //time not disable(except while zapped)

    public GameObject scoreMenu;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreBreakdown;
    public TMP_InputField nameField;
    [SerializeField]int score = 0;

    public TextMeshProUGUI scoreHud;
    public TextMeshProUGUI timeHud;


    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);

        zaps = 0;//
        time = 0f;//
        resets = 0;//
        deaths = 0;//
        refuels = 0;//
        spikings = 0;
        strandings = 0;//
        collisions = 0;//
        fuelBurned = 0f;//
        damageTaken = 0f;//
        distanceDragged = 0f;
        distanceConveyed = 0f;


        scoreMenu.SetActive(false);
    }

    void UpdateScore()
    {
        score = 0;
        //calculate the score

        score += zaps * 50;
        score += resets * 25;
        score += deaths * 75;
        score += strandings * 75;
        score += refuels * 25;
        score += collisions * 5;
        score += (int)fuelBurned * 10;
        score += (int)damageTaken * 2;
        score += (int)distanceDragged * 10;
        score -= (int)distanceConveyed * 50;
        score += (int)time;
        //take into account the difficulty multiplier

        score = (int)(score / DifficultyManager.mult);
    }

    private void Update()
    {
        UpdateScore();
        scoreHud.text = score.ToString();
        timeHud.text = ((int)time).ToString();
    }

    public void CalculateScore()
    {
        Debug.Log("calc score");

        string breakdown = zaps + "\n"
            + resets + "\n"
            + deaths + "\n"
            + strandings + "\n"
            + refuels + "\n"
            + collisions + "\n"
            + (int)fuelBurned + "\n"
            + (int)damageTaken + "\n"
            + (int)distanceDragged + "\n"
            + (int)distanceConveyed + "\n"
            + (int)time + "\n"
            + DifficultyManager.mult + "\n";

        scoreBreakdown.text = breakdown;

        UpdateScore();

        scoreText.text = score.ToString();

        scoreMenu.SetActive(true);
    }

    public void ReturnToMenu(bool submit)
    {
        if(submit)
        {
            ScoreIO.instance.NewLowScore(nameField.text, score);
        }
        SceneManager.LoadScene("MainMenu");
    }

}
