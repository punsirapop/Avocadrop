using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] Transform AvoCollection;
    [SerializeField] Transform UnusedCollection;
    [SerializeField] GameObject Avocado;

    ObjectPool<GameObject> pool;
    public int capacity = 20, max = 80;
    int msgReceivedCount;
    bool isReady = false, msgSent = false;
    List<Vector3> possibleSpaces = new List<Vector3>();

    private void OnEnable()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PhaseManager.OnPhaseChanged += HandlePhaseChanged;

        pool = new ObjectPool<GameObject>(
            () => { return Instantiate(Avocado, UnusedCollection); },
            avo => {
                avo.gameObject.SetActive(true);
                avo.transform.parent = AvoCollection;
                avo.GetComponent<Avocado>().isLock = 0;
                foreach(Transform t in avo.transform)
                {
                    t.gameObject.SetActive(false);
                }
            },
            avo => {
                avo.gameObject.SetActive(false);
                avo.transform.parent = UnusedCollection;
            },
            avo => { Destroy(avo.gameObject); },
            false, capacity, max);
    }

    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void HandlePhaseChanged(Phase phase)
    {
        if (phase == Phase.Spawn)
        {
            StartCoroutine(LoopSpawn());
            /*
            while (AvoCollection.childCount < capacity)
            {
                Debug.Log("Spawning an Avocado");
                SingleSpawn();
            }
            Debug.Log("Changing Phase from SpawnManager - spawner");
            PhaseManager.Instance.PhaseChange(Phase.PlayerAction);
            /*
            if(AvoCollection.childCount < capacity)
            {
                Debug.Log("Spawning an Avocado");
                SingleSpawn();
            }
            else
            {
                Debug.Log("Changing Phase from SpawnManager - spawner");
                PhaseManager.Instance.PhaseChange(Phase.PlayerAction);
            }
            */
        }
    }

    private IEnumerator LoopSpawn()
    {
        while (AvoCollection.childCount < capacity)
        {
            Debug.Log("Spawning an Avocado");
            GameObject avo = SingleSpawn();
            yield return new WaitWhile(() => avo.GetComponent<Avocado>().pleaseDrop);
        }
        Debug.Log("Changing Phase from SpawnManager - spawner");
        PhaseManager.Instance.PhaseChange(Phase.UpdateState);
    }

    private GameObject SingleSpawn()
    {
        isReady = false;
        msgSent = false;
        Debug.Log("Calling Search Function");
        InitiateSearch();
        while (true)
        {
            if (isReady)
            {
                GameObject avo = Spawn();
                // Debug.Log("Changing Phase from SpawnManager - after spawn");
                avo.SendMessage("PleaseDrop");
                // PhaseManager.Instance.PhaseChange(Phase.Drop);
                return avo;
            }
        }
    }

    private void InitiateSearch()
    {
        msgReceivedCount = 0;
        possibleSpaces.Clear();
        // Debug.Log("Sending messages to scanners");
        if (!msgSent)
        {
            BroadcastMessage("SearchPossibleSpace", SendMessageOptions.DontRequireReceiver);
            msgSent = true;
        }
    }

    public void CollectPossible(Vector3 position)
    {
        // Debug.Log("Received msg from scanner");
        msgReceivedCount++;
        if(position != Vector3.back)
        {
            possibleSpaces.Add(position);
        }
        // Debug.Log("Possible spaces - " + possibleSpaces.Count);

        if(msgReceivedCount == transform.childCount)
        {
            Debug.Log("Everybody sent msg, Ready to launch");
            isReady = true;
        }
    }

    private GameObject Spawn()
    {
        int spawnIndex = UnityEngine.Random.Range(0, possibleSpaces.Count);
        Debug.Log("Dropping in - " + possibleSpaces[spawnIndex]);

        GameObject avo = pool.Get();
        avo.transform.position = possibleSpaces[spawnIndex];

        return avo;
    }

    public void Despawn(GameObject avo)
    {
        Debug.Log("Despawning...");
        pool.Release(avo);
    }
}