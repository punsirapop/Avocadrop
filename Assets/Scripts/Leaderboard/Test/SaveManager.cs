using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveManager : ScoreManager
{
    [SerializeField] TMP_InputField player;

    public void CreateScore()
    {
        if (player.textComponent.text == "") player.textComponent.text = "Empty";

        string Name = player.textComponent.text;
        float Score = BoardState.currentScore;
        float Time = Timer.timeCount;

        Score score = new Score(Name, Score, Time);
        AddScore(score);
        SaveScore();
    }
}
