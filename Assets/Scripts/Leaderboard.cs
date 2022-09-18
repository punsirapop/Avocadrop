// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using LootLocker.Requests;
// using TMPro;

// public class Leaderboard : MonoBehaviour
// {
//     int leaderboardID = 7133;
//     public TextMeshProUGUI PlayerNames;
//     public TextMeshProUGUI PlayerScores;
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }
    
//     public IEnumerator SubmitScoreRoutine(int highscoretext)
//     {
//         bool done = false;
//         string PlayerNames = PlayerPrefs.GetString("name");

//         int highscoretext = PlayerPrefs.GetInt("highscore");
//         string PlayerScores = highscoretext.ToString();

//         LootLockerSDKManager.SubmitScore(PlayerNames, PlayerScores, leaderboardID, (response) =>
//         {
//             if (response.success)
//             {
//                 if(response.success)
//                 {
//                     Debug.Log("Successfully uploaded score");
//                 }
//                 else
//                 {
//                     Debug.Log("Failed" + response.Error);
//                     done = true;
//                 }
//             }
//         });
//         yield return new WaitWhile(() => done == false);
//     }
    
//     // public IEnumerator FetchTopHighscoresRoutine()
//     // {
//     //     bool done = false;
//     //     LootLockerSDKManager.GetScoreListMain(leaderboardID, 10, 0, (response) =>
//     //     {
//     //         if (response.success)
//     //         {
//     //             string tempPlayerNames = "Names\n";
//     //             string tempPlayerScores = "Scores\n";

//     //             LootLockerLeaderboardMember[] members = response.item;

//     //             for (int i = 0; i < members.Length; i++)
//     //             {
//     //                 tempPlayerNames += members[i].rank +". ";
//     //                 if(members[i].player.name != "")
//     //                 {
//     //                     tempPlayerNames += members[i].player.name;
//     //                 }
//     //                 else
//     //                 {
//     //                     tempPlayerNames += members[i].player.id;
//     //                 }
//     //                 tempPlayerScores += members[i].score + "\n";
//     //                 tempPlayerNames += "\n";
//     //             }
//     //             done = true;
//     //             PlayerNames.text = tempPlayerNames;
//     //         }
//     //         else
//     //         {
//     //             Debug.Log("Failed" + response.Error);
//     //             done = true;
//     //         }
//     //     });
//     //     yield return new WaitWhile(() => done == false);

//     // }

// }
