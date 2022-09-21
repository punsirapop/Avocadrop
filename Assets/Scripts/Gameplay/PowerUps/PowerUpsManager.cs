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
    [SerializeField] List<Sprite> PUSprites;

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
                pair.Value.GetComponent<SpriteRenderer>().sprite = PUSprites[index];
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



    // dont call while avo's moving
    // mode: 0 = plus, 1 = cross, 2 = whole
    // receive pos from outside later
    public void YeetusDeletus(int mode)
    {
        Vector2 pos = new Vector2(4, 4);
        mode += 4;
        PURanges[mode].transform.position = pos;
        PURanges[mode].SetActive(true);
        PURanges[mode].SendMessage("Explode");
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
