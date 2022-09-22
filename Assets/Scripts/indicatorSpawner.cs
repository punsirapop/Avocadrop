using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class indicatorSpawner : MonoBehaviour
{
    public GameObject crateIndicator;
    public const int n = 9;
    public const int m = 9;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                GameObject indicator =  Instantiate(crateIndicator, transform);
                indicator.transform.position = new Vector3(i,j,0);
            }

        }
    }

}
