using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject Panel;

    public void ToGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void ToScore()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenPanel()
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
