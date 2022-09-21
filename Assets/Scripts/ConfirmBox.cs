using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBox : MonoBehaviour
{
    [SerializeField] Button button;

    public void SetDes(int des)
    {
        button.onClick.AddListener(() => Buttons.Open(des));
    }
}
