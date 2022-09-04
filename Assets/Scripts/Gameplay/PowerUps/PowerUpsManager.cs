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
            GameObject newPU = Instantiate(PUPrefab, p);
            newPU.SetActive(false);
            PUList.Add(p, newPU);
        }
    }

    public void getPowerUp(int index)
    {
        foreach (KeyValuePair<Transform, GameObject> pair in PUList)
        {
            // GameObject range = pair.Value.GetComponent<PowerUps>().Range;
            pair.Value.TryGetComponent(out PowerUps PUComp);
            if (PUComp.Range == null)
            {
                pair.Value.SetActive(true);
                // pair.Value.transform.position = pair.Key.position;
                pair.Value.SendMessage("SetRange", PURanges[index]);
                break;
            }
        }

        /*
        if (PUList.ContainsValue(PUPrefab))
        {
            Debug.Log("FOUND");
            /*
            foreach(KeyValuePair<Transform, GameObject> pair in PUList)
            {
                if(pair.Value == null)
                {
                    pair.Value.GetComponent<PowerUps>().Range = PURanges[index];
                    pair.Value.SetActive(true);
                    break;
                }
            }
            
        }
        */
    }

    /*
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
    */
}
