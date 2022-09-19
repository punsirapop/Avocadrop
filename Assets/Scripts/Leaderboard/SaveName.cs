using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveName : MonoBehaviour
{
    public InputField textBox;
    // public Text highscoretext;

    int count;
    int getcount;

    public void clickSaveButton(){

        getcount=PlayerPrefs.GetInt("savecount");
        PlayerPrefs.SetString("name", textBox.text);
        Debug.Log("Your Name is " + PlayerPrefs.GetString("name"));                
        PlayerPrefs.SetInt("savecount", getcount);
        Debug.Log("Count Success" + PlayerPrefs.GetInt("savecount"));
        getcount++;
        PlayerPrefs.SetInt("savecount", getcount);

    }

}
