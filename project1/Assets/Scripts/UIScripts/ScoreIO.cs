using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class ScoreIO : MonoBehaviour
{
    public static ScoreIO instance;

    [SerializeField]
    List<Score> lowScores = new List<Score>();

    public Score[] GetTopScores(int count)
    {
        SortScores();

        if (lowScores.Count > count) return lowScores.GetRange(0, count).ToArray();
        else return lowScores.ToArray();
    }

    public void NewLowScore(string name, int score)
    {
        if(score >= 0)
            lowScores.Add(new Score(name, score));
    }

    void SortScores()
    {
        lowScores = lowScores.OrderBy(e => e.score).ToList();
    }


    //load from file if it exists
    private void Awake()
    {
        Debug.Log("ScoreIO Awake()");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                FileStream file = File.Open(Application.persistentDataPath + "/low_scores.save", FileMode.Open);
                lowScores = new List<Score>((Score[])bf.Deserialize(file));
                file.Close();
            }catch(FileNotFoundException IOE)
            {
                Debug.LogWarning(IOE.Message);
            }
        }
        else DestroyImmediate(gameObject);
    }

    //save to file
    private void OnApplicationQuit()
    {
        if (lowScores.Count > 0)
        {
            SortScores();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/low_scores.save", FileMode.Create);

            bf.Serialize(file, lowScores.ToArray());

            file.Close();
        }
    }

    [System.Serializable]
    public class Score
    {
        public string name;
        public int score;

        public Score(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
