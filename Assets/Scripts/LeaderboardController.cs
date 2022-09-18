using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;


public class LeaderboardController : MonoBehaviour
{

    public Text PlayerName, PlayerScore;
    public int highscore_, leaderboardID = 7133;

    private void Start()
    {
     LootLockerSDKManager.StartSession("Player", (response) =>
        {
            if (response.success)
            {
                Debug.Log("Success session");
            }
            else
            {
                Debug.Log("Failed session");
            }
        });
    }
    //     string memberID = "20";
    //     int score = 1000;

    //     LootLockerSDKManager.SubmitScore(memberID, score, leaderboardID, (response) =>
    // {
    //     if (response.statusCode == 200)
    //     {
    //         Debug.Log("Save data Successful");
    //     } 
    //     else
    //     {
    //         Debug.Log("failed: " + response.Error);
    //     }
    // });

    // public void SubmitScore()
    // {
    //     PlayerName.text = PlayerPrefs.GetString("name");
    //     highscore_ = PlayerPrefs.GetInt("highscore");
    //     PlayerScore.text = highscore_.ToString();
    //     // PlayerScore.text = PlayerPrefs.GetInt("highscore");

    //     LootLockerSDKManager.SubmitScore(PlayerName.text, PlayerScore.text, leaderboardID, (response) =>
    //     {
    //         if (response.success)
    //         {
    //             Debug.Log("Success submit");
    //         }
    //         else
    //         {
    //             Debug.Log("Failed submit");
    //         }
    //     });

    // }
}
