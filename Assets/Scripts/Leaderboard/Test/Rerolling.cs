using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rerolling : MonoBehaviour
{
    private void Update()
    {
        if (BoardState.isRerolling)
        {
            gameObject.SetActive(true);
        }
        
    }
}
