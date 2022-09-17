using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Multiplier : MonoBehaviour
{
    float lockPos = 0;
    public int multiplierAmount = 1;
    public SpriteRenderer spriteRenderer;
    public Sprite[] multiplierSpriteArray;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPos, lockPos);

    }

    public void setMultiplierAmount(int amount)
    {
        multiplierAmount = amount;
        changeSprite(amount);

    }
    void changeSprite(int amount)
    {
        spriteRenderer.sprite = multiplierSpriteArray[amount-2];
    }
}
