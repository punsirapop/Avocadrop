using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveManager : ScoreManager
{
    [SerializeField] TextMeshProUGUI player;

    public void CreateScore()
    {
        if (player.text == null) player.text = "Empty";

        string Name = player.text;
        float Score = BoardState.currentScore;
        float Time = Timer.timeCount;

        Score score = new Score(Name, Score, Time);
        AddScore(score);
        SaveScore();
    }
}
