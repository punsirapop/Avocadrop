using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class SaveManager : ScoreManager
{
    [SerializeField] TMP_InputField player;

    public void CreateScore()
    {
        string Name = (String.IsNullOrWhiteSpace(player.text)) ? "Empty" : player.text;
        float Score = BoardState.currentScore;
        float Time = Timer.timeCount;

        Score score = new Score(Name, Score, Time);
        AddScore(score);
        SaveScore();
    }
}
