using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    [SerializeField] PlatformEffector2D effector;
    [SerializeField] SpriteRenderer renderer;
    bool occupied = false;
    Color color;

    private void Start()
    {
        color = renderer.color;
    }

    void Update()
    {
        if (occupied)
        {
            renderer.color = Color.gray;
        }
        else
        {
            renderer.color = color;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.transform.position);
        if (!occupied)
        {
            occupied = true;
        }
        else
        {
            effector.rotationalOffset = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        occupied = false;
        // effector.rotationalOffset = 180;
    }
}
