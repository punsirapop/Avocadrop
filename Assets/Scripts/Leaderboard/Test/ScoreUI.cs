using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;

    private void Start()
    {
        var scores = scoreManager.GetHighScore().ToArray();
        Debug.Log(scores);
        int length = (scores.Length < 10) ? scores.Length : 10;
        for (int i = 0; i < length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            float minute = Mathf.FloorToInt(scores[i].time / 60);
            float second = Mathf.FloorToInt(scores[i].time % 60);

            row.player.text = scores[i].player;
            row.score.text = scores[i].score.ToString();
            row.time.text = string.Format("{0:00}:{1:00}", minute, second);
        }
    }
}
