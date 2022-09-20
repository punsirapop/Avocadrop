using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
