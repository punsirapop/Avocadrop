using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PowerUpsSinglePool : MonoBehaviour
{
    [SerializeField] GameObject Single;

    int capacity = 20, max = 20;
    public static ObjectPool<GameObject> singlePool;

    private void OnEnable()
    {
        if (singlePool == null)
        {
            Debug.Log("GEN NEW POOL");

            singlePool = new ObjectPool<GameObject>(
            () => { return Instantiate(Single, transform); },
            s => {
                s.gameObject.SetActive(true);
            },
            s => {
                s.gameObject.SetActive(false);
            },
            s => {
                s.gameObject.SetActive(false);
            },
            false, capacity, max);
        }
    }
}
