using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockGuage : MonoBehaviour
{
    [SerializeField] Sprite[] guage;
    [SerializeField] Image guageSprite;

    private void Update()
    {
        guageSprite.sprite = guage[BoardState.rotationSinceLastMatch];
    }
}
