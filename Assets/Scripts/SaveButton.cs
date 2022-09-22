using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI t;
    [SerializeField] Button b;
    public void DisableSave()
    {
        t.SetText("Saved");
        b.interactable = false;
    }
}
