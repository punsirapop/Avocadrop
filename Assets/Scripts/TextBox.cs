using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextBox : MonoBehaviour
{
    public Text NameBox;
    public Text ScoreBox;

    // public TextMeshProUGUI PlayerNames;
    // public TextMeshProUGUI PlayerScores;

    int highscoretext;
    // Start is called before the first frame update
    public void Start()
    {
        NameBox.text = PlayerPrefs.GetString("name");
        highscoretext = PlayerPrefs.GetInt("highscore");
        ScoreBox.text = highscoretext.ToString();
        

    }
    
        
}

