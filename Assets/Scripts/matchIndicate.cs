using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matchIndicate : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    Color color;
    private void Start()
    {
        color = spriteRenderer.color;

    }

    // Update is called once per frame
    void Update()
    {
        Collider2D avocadoFound = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Avocado"));

        if (avocadoFound)
        {
            if (avocadoFound.gameObject.GetComponent<Avocado>().isPartOfMatch)
            {
                spriteRenderer.color = Color.black;
            }
            else
            {
                spriteRenderer.color = color;
            }
        }
        else
        {
            spriteRenderer.color = color;
        }
    }
}
