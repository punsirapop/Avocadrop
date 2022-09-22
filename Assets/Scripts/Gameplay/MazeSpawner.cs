using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeSpawner : MonoBehaviour
{
    public static MazeSpawner Instance;

    [SerializeField] GameObject Box;
    [SerializeField] Transform BoxHolder;

    int n = 9, m = 9, wallCount = 0;

    public int revealedSoFar = 0;

    List<GameObject> visitedList = new List<GameObject>();
    List<GameObject> toVisitList = new List<GameObject>();
    List<GameObject> toDestroyList = new List<GameObject>();
    List<Transform> childList = new List<Transform>();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void HandlePhaseChanged(Phase phase)
    {
        if(phase == Phase.Preparation)
        {
            StartCoroutine(Initialize());
        }
    }

    private IEnumerator Initialize()
    {
        int row = Random.Range(0, n);
        int col = Random.Range(0, m);

        Vector2 pos = new Vector2(row, col);
        Collider2D gridFound = Physics2D.OverlapCircle(pos, .1f, LayerMask.GetMask("Grid"));
        visitedList.Add(gridFound.gameObject);
        Debug.Log("START AT: " + gridFound.transform.position);
        yield return StartCoroutine(ScanGridAround(pos, false));
        yield return StartCoroutine(DigEmOut());
        foreach(Transform t in transform)
        {
            wallCount += t.childCount;
        }
        Debug.Log(wallCount);

        Collider2D[] wallsFound;
        int layermask = ~(LayerMask.GetMask("Obstacle")) & ~(LayerMask.GetMask("Wall"));
        List<Vector2> deadEnd = new List<Vector2>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Vector2 here = new Vector2(i, j);
                wallsFound = Physics2D.OverlapCircleAll(here, .5f, -layermask);
                if (wallsFound.Length == 3)
                {
                    deadEnd.Add(here);
                    // Instantiate(Box, here, Quaternion.identity, BoxHolder);
                }
            }
        }
        for (int k = 0; k < Random.Range(5,10); k++)
        {
            int index = Random.Range(0, deadEnd.Count);
            wallsFound = Physics2D.OverlapCircleAll(deadEnd[index], .5f, LayerMask.GetMask("Obstacle"));
            foreach(Collider2D w in wallsFound)
            {
                Destroy(w.gameObject);
            }
            Instantiate(Box, deadEnd[index], Quaternion.identity, BoxHolder);
        }

        yield return StartCoroutine(HideMaze());

        Debug.Log(childList.Count);
        foreach (Transform t in transform)
        {
            childList.AddRange(t.Cast<Transform>().ToList());
        }
        childList.RemoveAll(x => x.gameObject.activeSelf);
        Debug.Log(childList.Count);

        Reveal(5);

        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
        PhaseManager.Instance.PhaseChange(Phase.Spawn);
    }

    // true = want non-visited, false = want visited
    private IEnumerator ScanGridAround(Vector2 grid, bool mode)
    {
        toDestroyList.Clear();

        Collider2D[] gridsFound = Physics2D.OverlapCircleAll(grid, .6f, LayerMask.GetMask("Grid"));

        foreach (Collider2D s in gridsFound)
        {
            if (!visitedList.Contains(s.gameObject) ^ mode)
            {
                // Debug.Log("adding stuffs");
                if (mode)
                {
                    Debug.Log("adding walls");
                    toDestroyList.Add(ScanWallBetween(s.transform.position, grid));
                }
                else if(!toVisitList.Contains(s.gameObject))
                {
                    Debug.Log("adding grids " + s.transform.position);
                    toVisitList.Add(s.gameObject);
                }
            }
        }

        if (mode)
        {
            Debug.Log("Destroyable wall(s): " + toDestroyList.Count);
        }
        else
        {
            Debug.Log("Available grid(s): " + toVisitList.Count);
        }

        yield return null;
    }

    private GameObject ScanWallBetween(Vector2 visited, Vector2 me)
    {
        Debug.Log(me + "" + visited);
        RaycastHit2D wallFound = Physics2D.Raycast(me, visited - me, .6f, LayerMask.GetMask("Obstacle"));
        return wallFound.collider.gameObject;
    }

    private IEnumerator DigEmOut()
    {
        while (toVisitList.Count > 0)
        {
            int gridIndex = Random.Range(0, toVisitList.Count);
            Vector2 destination = toVisitList[gridIndex].transform.position;
            Debug.Log(gridIndex + "/" + toVisitList.Count + " " + destination);
            yield return StartCoroutine(ScanGridAround(destination, true));
            Destroy(toDestroyList[Random.Range(0, toDestroyList.Count)]);
            visitedList.Add(toVisitList[gridIndex]);
            yield return StartCoroutine(ScanGridAround(destination, false));
            toVisitList.RemoveAt(gridIndex);
        }
    }

    private IEnumerator HideMaze()
    {
        foreach(Transform t in transform)
        {
            foreach(Transform u in t.GetComponentsInChildren<Transform>())
            {
                u.gameObject.SetActive(false);
            }
            t.gameObject.SetActive(true);
        }

        transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
        foreach(Transform v in transform.GetChild(transform.childCount - 1))
        {
            v.gameObject.SetActive(true);
        }

        yield return null;
    }

    public void Reveal(int num)
    {
        revealedSoFar += num;
        num = (num < childList.Count) ? num : childList.Count;

        for (int i = 0; i < num; i++)
        {
            int random = Random.Range(0, childList.Count);
            childList[random].gameObject.SetActive(true);
            childList.RemoveAt(random);
        }
    }

    /*
    public void Reveal(int num)
    {
        Debug.Log("Revealing...");
        for (int i = 0; i < num; i++)
        {
            Transform child;
            Transform child2;
            while (true)
            {
                child = transform.GetChild(Random.Range(0, transform.childCount - 1));
                if (child.childCount > 0)
                {
                    child2 = child.GetChild(Random.Range(0, child.childCount));
                    if (!child2.gameObject.activeSelf)
                    {
                        child2.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
    */
}