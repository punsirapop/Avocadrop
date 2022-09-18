using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveName : MonoBehaviour
{
    public InputField textBox;
    // public Text highscoretext;

    public void clickSaveButton(){
        PlayerPrefs.SetString("name", textBox.text);
        Debug.Log("Your Name is " + PlayerPrefs.GetString("name"));

    }
}
