using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetGuage : MonoBehaviour
{
    [SerializeField] Sprite[] guage;
    [SerializeField] Image guageSprite;

    private void Update()
    {
        int index = Mathf.RoundToInt((BoardState.timeToGo - Time.fixedTime)/4);
        guageSprite.sprite = guage[index];
    }
}
