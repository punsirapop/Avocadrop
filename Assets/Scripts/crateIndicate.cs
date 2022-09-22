using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crateIndicate : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameObject left;
    GameObject right;
    GameObject top;
    GameObject bottom;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        disableEverything();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhaseManager.Instance.phase != Phase.CheckExplode)
        {
            disableEverything();
            return;
        }

        Collider2D avocadoFound = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Avocado"));

        if (avocadoFound)
        {
            if (avocadoFound.gameObject.GetComponent<Avocado>().aboutToExplode)
            {
                Color colorToCheck = BoardState.currentColorMatch;
                spriteRenderer.color = colorToCheck;
                spriteRenderer.enabled = true;
                //checkSides();
            }
            else
            {
                spriteRenderer.enabled = false;
                left.GetComponent<SpriteRenderer>().enabled = false;
                right.GetComponent<SpriteRenderer>().enabled = false;
                top.GetComponent<SpriteRenderer>().enabled = false;
                bottom.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
            left.GetComponent<SpriteRenderer>().enabled = false;
            right.GetComponent<SpriteRenderer>().enabled = false;
            top.GetComponent<SpriteRenderer>().enabled = false;
            bottom.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void disableEverything()
    {
        spriteRenderer.enabled = false;
        left = transform.GetChild(0).gameObject;
        right = transform.GetChild(1).gameObject;
        top = transform.GetChild(2).gameObject;
        bottom = transform.GetChild(3).gameObject;
        left.GetComponent<SpriteRenderer>().enabled = false;
        right.GetComponent<SpriteRenderer>().enabled = false;
        top.GetComponent<SpriteRenderer>().enabled = false;
        bottom.GetComponent<SpriteRenderer>().enabled = false;
    }

    void checkSides()
    {
        Color colorToCheck = BoardState.currentColorMatch;
        Collider2D leftAvo = Physics2D.OverlapCircle(transform.position + Vector3.left, 0.1f, LayerMask.GetMask("Avocado"));
        Collider2D rightAvo = Physics2D.OverlapCircle(transform.position + Vector3.right, 0.1f, LayerMask.GetMask("Avocado"));
        Collider2D upAvo = Physics2D.OverlapCircle(transform.position + Vector3.up, 0.1f, LayerMask.GetMask("Avocado"));
        Collider2D downAvo = Physics2D.OverlapCircle(transform.position + Vector3.down, 0.1f, LayerMask.GetMask("Avocado"));
        if (leftAvo && leftAvo.GetComponent<Avocado>().color.Equals(colorToCheck))
        {
            left.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            left.GetComponent<SpriteRenderer>().color = colorToCheck;
            left.GetComponent<SpriteRenderer>().enabled = true;
        }
        if (rightAvo && rightAvo.GetComponent<Avocado>().color.Equals(colorToCheck))
        {
            right.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            right.GetComponent<SpriteRenderer>().color = colorToCheck;
            right.GetComponent<SpriteRenderer>().enabled = true;
        }
        if (upAvo && upAvo.GetComponent<Avocado>().color.Equals(colorToCheck))
        {
            top.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            top.GetComponent<SpriteRenderer>().color = colorToCheck;
            top.GetComponent<SpriteRenderer>().enabled = true;
        }
        if (downAvo && downAvo.GetComponent<Avocado>().color.Equals(colorToCheck))
        {
            bottom.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            bottom.GetComponent<SpriteRenderer>().color = colorToCheck;
            bottom.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
