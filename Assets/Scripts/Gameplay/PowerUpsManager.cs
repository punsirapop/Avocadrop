using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PowerUpsManager : MonoBehaviour
{
    public static PowerUpsManager Instance;

    [SerializeField] List<Transform> PUSpaces;
    [SerializeField] GameObject PUPrefab;
    [SerializeField] List<GameObject> PURanges;

    Dictionary<Transform, GameObject> PUList = new Dictionary<Transform, GameObject>();
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        foreach(Transform p in PUSpaces)
        {
            PUList.Add(p, null);
        }
    }

    public void getPowerUp(int index)
    {
        if (PUList.ContainsValue(null))
        {
            foreach(KeyValuePair<Transform, GameObject> pair in PUList)
            {
                if(pair.Value == null)
                {
                    GameObject newPU = Instantiate(PUPrefab, pair.Key);
                    newPU.GetComponent<PowerUps>().Range = PURanges[index];
                    PUList[pair.Key] = newPU;
                    break;
                }
            }
        }
    }

    public void usePowerUp(GameObject PU)
    {
        foreach (KeyValuePair<Transform, GameObject> pair in PUList)
        {
            if (pair.Value == PU)
            {
                PUList[pair.Key] = null;
                break;
            }
        }
    }
}
