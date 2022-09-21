using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    /*
    0 - main
    1 - game
    2 - score
    3 - exit
    */

    [SerializeField] GameObject ConfirmationBox;

    public static void Open(int des)
    {
        if (des == 3)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(des);
        }
    }

    public void OpenConfirm(int des)
    {
        ConfirmationBox?.SetActive(true);
        ConfirmationBox?.SendMessage("SetDes", des);
    }

    public void CloseConfirm()
    {
        ConfirmationBox?.SetActive(false);
    }
}
