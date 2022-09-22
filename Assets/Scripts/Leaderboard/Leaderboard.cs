using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    List<string> playerNames = new List<string>();
    List<string> playerScores = new List<string>();

    string loadedScore;
    public void loadScore()
    {
        loadedScore = PlayerPrefs.GetString("name")+PlayerPrefs.GetInt("highscore");

        if (!loadedScore.Equals(""))
        {   
            loadedScore = PlayerPrefs.GetString("playerAndScore","");

            for (int i = 0; i < PlayerPrefs.GetInt("savecount"); i++)
            {
                string playerName = PlayerPrefs.GetString("name");
                int highscoretext = PlayerPrefs.GetInt("highscore");
                string score = highscoretext.ToString();
                playerNames.Add(playerName);
                playerScores.Add(score);
                Debug.Log("getscore" +PlayerPrefs.GetInt("savecount"));

            string newScore = playerName + ":" + highscoretext;
            playerNames.Add(playerName);
            playerScores.Add(highscoretext.ToString());
            if (loadedScore.Equals(""))
            {
                loadedScore = newScore;
            }
            else
            {
                loadedScore = loadedScore + "###" + newScore;
            }
                PlayerPrefs.SetString("playerAndScore", loadedScore);

                Debug.Log("Player: "+playerNames[i]+" Score: " + playerScores[i]);
                Debug.Log(i);
            }
        }

    }


}
