using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Transform AvoCollection;
    [SerializeField] Transform UnusedCollection;
    [SerializeField] GameObject Avocado;

    ObjectPool<GameObject> pool;
    int capacity = 20, max = 80;
    int msgReceivedCount;
    bool isReady = false, msgSent = false;
    List<Vector3> possibleSpaces = new List<Vector3>();

    private void OnEnable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
        pool = new ObjectPool<GameObject>(
            () => { return Instantiate(Avocado, UnusedCollection); },
            avo => {
                avo.gameObject.SetActive(true);
                avo.transform.parent = AvoCollection;
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

    private void Start()
    {
        /*
        pool = new ObjectPool<GameObject>(
            () => { return Instantiate(Avocado, UnusedCollection); },
            avo => { avo.gameObject.SetActive(true);
                avo.transform.parent = AvoCollection;
            },
            avo => { avo.gameObject.SetActive(false);
                avo.transform.parent = UnusedCollection;
            },
            avo => { Destroy(avo.gameObject); },
            false, capacity, max);
        */
    }

    private void HandlePhaseChanged(Phase phase)
    {
        if (phase == Phase.Spawn)
        {
            if(AvoCollection.childCount < capacity)
            {
                Debug.Log("Spawning an Avocado");
                SingleSpawn();
            }
            else
            {
                PhaseManager.Instance.PhaseChange(Phase.PlayerAction);
            }
        }
    }

    private void SingleSpawn()
    {
        isReady = false;
        msgSent = false;
        Debug.Log("Calling Search Function");
        InitiateSearch();
        while (true)
        {
            if (isReady)
            {
                Spawn();
                PhaseManager.Instance.PhaseChange(Phase.Drop);
                break;
            }
        }
    }

    private void InitiateSearch()
    {
        msgReceivedCount = 0;
        possibleSpaces.Clear();
        Debug.Log("Sending messages to scanners");
        if (!msgSent)
        {
            BroadcastMessage("SearchPossibleSpace", SendMessageOptions.DontRequireReceiver);
            msgSent = true;
        }
    }

    public void CollectPossible(Vector3 position)
    {
        Debug.Log("Received msg from scanner");
        msgReceivedCount++;
        if(position != Vector3.back)
        {
            possibleSpaces.Add(position);
        }
        Debug.Log("Possible spaces - " + possibleSpaces.Count);

        if(msgReceivedCount == transform.childCount)
        {
            Debug.Log("Everybody sent msg, Ready to launch");
            isReady = true;
        }
    }

    private void Spawn()
    {
        int spawnIndex = Random.Range(0, possibleSpaces.Count);
        Debug.Log("Dropping in - " + possibleSpaces[spawnIndex]);

        GameObject avo = pool.Get();
        avo.transform.position = possibleSpaces[spawnIndex];
    }
}