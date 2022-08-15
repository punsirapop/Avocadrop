using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avocado : MonoBehaviour
{
    Vector2 destination;

    private void Start()
    {
        Drop();
    }
    void Drop()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 8f);
        if(hit.collider == null)
        {
            Debug.Log(4f);
        }
        else
        {
            Debug.Log(hit.collider.transform.position.y);
        }
    }
}