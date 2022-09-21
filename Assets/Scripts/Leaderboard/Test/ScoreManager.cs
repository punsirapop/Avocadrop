using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System;

[Serializable]
public class ScoreData
{
    public List<Score> scores = new List<Score>();

    public ScoreData()
    {
        this.scores = new List<Score>();
    }
}

public class ScoreManager : MonoBehaviour
{
    ScoreData sd;
    Score score;
    string path = "";
    string persistentPath = "";

    private void Awake()
    {
        SetPath();
        LoadScore();
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            CreateScore();
            AddScore(score);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SaveScore();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadScore();
        }
    }

    private void CreateScore()
    {
        Debug.Log("creating new score...");
        score = new Score("Test", 200f, 200f);
    }
    */

    private void SetPath()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar
            + "Saves"+ Path.AltDirectorySeparatorChar + "SaveData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        Debug.Log("Save path: " + path);
    }

    public IEnumerable<Score> GetHighScore()
    {
        return sd.scores.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
        Debug.Log("score added");
    }

    public void SaveScore()
    {
        string savePath = path;
        string json = JsonUtility.ToJson(sd);
        Debug.Log("writing this - " + json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadScore()
    {
        if (File.Exists(path))
        {
            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            Debug.Log(json);

            sd = JsonUtility.FromJson<ScoreData>(json);
        }
        else
        {
            sd = new ScoreData();
        }
    }
}