using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Score
{
    public string player;
    public float score;
    public float time;

    public Score(string Player, float Score, float Time)
    {
        this.player = Player;
        this.score = Score;
        this.time = Time;
    }
}
