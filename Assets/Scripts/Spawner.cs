using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour
{
    bool isFound;

    public void SearchPossibleSpace()
    {
        isFound = false;

        Debug.Log(transform.position.x + " - Searching");
        int layermask = ~(LayerMask.GetMask("Avocado")) & ~(LayerMask.GetMask("Grid"));
        for(int i = 1; i < 9; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, i, -layermask);

            if(hits.Length < i*2)
            {
                isFound = true;
                Debug.Log(transform.position.x + " - Found a space - " + hits[hits.Length-1].transform.position);
                SendMessageUpwards("CollectPossible", hits[hits.Length - 1].transform.position);
                break;
            }
        }
        
        if (!isFound)
        {
            Debug.Log(transform.position.x + "Didn't find any available space");
            SendMessageUpwards("CollectPossible", Vector3.back);
        }
    }
}
