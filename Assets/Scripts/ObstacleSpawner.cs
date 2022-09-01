using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> tier0 = new List<GameObject>();
    [SerializeField] List<GameObject> tier1 = new List<GameObject>();
    [SerializeField] List<GameObject> tier2 = new List<GameObject>();
    [SerializeField] List<GameObject> tier3 = new List<GameObject>();

    int[,] gridLayout = { {-1, -2, -3 },
                          {-4, -5, -6 },
                          {-7, -8, -9 }};
    int[,] gridAngle = { {-1, -2, -3 },
                         {-4, -5, -6 },
                         {-7, -8, -9 }};

    bool isPass = false;

    private void OnEnable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }

    /*
    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }
    */

    void HandlePhaseChanged(Phase phase)
    {
        if(phase == Phase.Preparation)
        {
            for(int row = 0; row < 3; row++)
            {
                for(int col = 0; col < 3; col++)
                {
                    isPass = false;
                    while (!isPass)
                    {
                        gridLayout[row, col] = Random.Range(0, 4);
                        isPass = Compare(gridLayout, row, col);
                    }
                    isPass = false;
                    while (!isPass)
                    {
                        gridAngle[row, col] = Random.Range(0, 4);
                        isPass = Compare(gridAngle, row, col);
                    }
                }
            }
            int i = 0;
            int[] layout = gridLayout.Cast<int>().ToArray();
            int[] angle = gridAngle.Cast<int>().ToArray();

            List<GameObject> listToGen = new List<GameObject>();
            foreach (Transform obstaclePos in transform)
            {
                switch (layout[i])
                {
                    case 0:
                        listToGen = new List<GameObject>(tier0);
                        break;
                    case 1:
                        listToGen = new List<GameObject>(tier1);
                        break;
                    case 2:
                        listToGen = new List<GameObject>(tier2);
                        break;
                    case 3:
                        listToGen = new List<GameObject>(tier3);
                        break;
                }

                Instantiate(listToGen[Random.Range(0, listToGen.Count)], obstaclePos.position,
                    Quaternion.AngleAxis(angle[i] * 90, Vector3.forward), obstaclePos);
                
                i++;
                /*
                Instantiate(obstacles[obsIndex], obstaclePos.position,
                    Quaternion.AngleAxis(obsAngle * 90, Vector3.forward),
                    obstaclePos);
                */
            }
        }

        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
        PhaseManager.Instance.PhaseChange(Phase.Spawn);
    }

    bool Compare(int[,] array2d, int row, int col)
    {
        List<int> rowList, colList;

        colList = Enumerable.Range(0, array2d.GetLength(0))
        .Select(x => array2d[x, col]).ToList();

        rowList = Enumerable.Range(0, array2d.GetLength(1))
        .Select(x => array2d[row, x]).ToList();

        return (rowList.Distinct().Count() == rowList.Count()) && (colList.Distinct().Count() == colList.Count());
    }
}
