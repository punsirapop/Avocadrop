using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardState : MonoBehaviour
{
    public static BoardState Instance;
    public bool fallingDone;
    public const int n = 9;
    public const int m = 9;
    //Vector3 worldPosition;

    // stores information about which cell
    // are already visited in a particular BFS
    static readonly int[][] visited = ReturnRectangularIntArray(n, m);

    // result stores the final result grid
    static readonly int[][] result = ReturnRectangularIntArray(n, m);

    // stores the count of cells in the
    // largest connected component
    public static int COUNT;

    public Camera cam;
    GameObject[][] gameState;
    static GameObject[] currentMatch = new GameObject[n*m];

    //void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.DownArrow))
    //    {
    //        Debug.Log("Forcing update board state");
    //        updateState();
    //    }
    //    if (Input.GetKey(KeyCode.Mouse0))
    //    {
    //        Vector3 mousePos = Input.mousePosition;
    //        mousePos.z = Camera.main.nearClipPlane;
    //        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
    //        Debug.Log(worldPosition);
    //    }

    //}

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public static GameObject[][] ReturnRectangularGameObjectArray(int size1,int size2)
    {
        GameObject[][] newArray = new GameObject[size1][];
        for (int array1 = 0; array1 < size1; array1++)
            {
                newArray[array1] = new GameObject[size2];
            }

         return newArray;
     }


    public void updateState()
    {
        int count = 0;
        //reset arrays
        gameState = ReturnRectangularGameObjectArray(9, 9);
        foreach (GameObject avocado in currentMatch)
            {
                if (avocado)
                {
                    avocado.GetComponent<Avocado>().isPartOfMatch = false;
                }
            }
        currentMatch = new GameObject[30];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Debug.Log("=========== Checking at: (" + i + "," + j + ") ===========");

                Collider2D avocadoFound = Physics2D.OverlapCircle(new Vector2(i, j), 0.1f, LayerMask.GetMask("Avocado"));

                if (avocadoFound)
                {
                    count++;
                    Debug.Log(avocadoFound.gameObject.GetComponent<Transform>().position);
                    //Debug.Log(avocadoFound.gameObject.GetComponent<Avocado>().color);
                    gameState[i][j] = avocadoFound.gameObject;
                }

            }
        }

        //checking things in gameState
        Debug.Log("=========== total found : " + count + "===========");
        //Debug.Log("=========== checking in list ===========");
        //for (int i = 0; i < gameState.Length; i++)
        //{
        //    for (int j = 0; j < gameState[i].Length; j++)
        //    {
        //        Debug.Log("=========== Checking at: (" + i + "," + j + ") ===========");
        //        if (gameState[i][j] != null)
        //        {
        //            Debug.Log(gameState[i][j].GetComponent<Avocado>().color);
        //        }
        //    }
        //}

        computeLargestConnectedGrid(gameState);

        



    }

    void getLargestConnection()
    {
        
    }

    void detectWhatPattern()
    {

    }

    void getFlaggedCircle()
    {

    }
    

    // Function checks if a cell is valid i.e
    // it is inside the grid and equal to the key
    internal static bool is_valid(int x, int y, Color key, GameObject[][] input)
    {
        if (x < n && y < m && x >= 0 && y >= 0)
        {
            if (input[x][y] != null)
            {
                if (visited[x][y] == 0 && input[x][y].GetComponent<Avocado>().color.Equals(key))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    // BFS to find all cells in
    // connection with key = input[i][j]
    public static void BFS(GameObject x, GameObject y, int i, int j, GameObject[][] input)
    {
        // terminating case for BFS
        if (input[i][j] == null || x == null || y == null || !x.GetComponent<Avocado>().color.Equals(y.GetComponent<Avocado>().color))
        {
            return;
        }

        visited[i][j] = 1;
        COUNT++;

        // x_move and y_move arrays
        // are the possible movements
        // in x or y direction
        int[] x_move = new int[] { 0, 0, 1, -1 };
        int[] y_move = new int[] { 1, -1, 0, 0 };

        // checks all four points
        // connected with input[i][j]
        for (int u = 0; u < 4; u++)
        {
            if (is_valid(i + y_move[u], j + x_move[u], x.GetComponent<Avocado>().color, input))
            {
                BFS(input[i][j], input[i + y_move[u]][j + x_move[u]], i + y_move[u], j + x_move[u], input);
            }
        }
    }

    // called every time before
    // a BFS so that visited
    // array is reset to zero
    internal static void reset_visited()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                visited[i][j] = 0;
            }
        }
    }

    // If a larger connected component is
    // found this function is called to
    // store information about that component.
    internal static void reset_result(Color key, GameObject[][] input)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (visited[i][j] == 1 && input[i][j].GetComponent<Avocado>().color.Equals(key))
                {
                    result[i][j] = visited[i][j];
                }
                else
                {
                    result[i][j] = 0;
                }
            }
        }
    }

    // function to calculate the
    // largest connected component
    public static void computeLargestConnectedGrid(GameObject[][] input)
    {
        int current_max = int.MinValue;
        Color bestColor = Color.black;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (input[i][j] != null)
                {
                    reset_visited();
                    COUNT = 0;

                    // checking cell to the right
                    if (j + 1 < m)
                    {
                        BFS(input[i][j], input[i][j + 1], i, j, input);
                    }

                    // updating result
                    if (COUNT >= current_max)
                    {
                        current_max = COUNT;
                        bestColor = input[i][j].GetComponent<Avocado>().color;
                        reset_result(input[i][j].GetComponent<Avocado>().color, input);
                    }
                    reset_visited();
                    COUNT = 0;

                    // checking cell downwards
                    if (i + 1 < n)
                    {
                        BFS(input[i][j], input[i + 1][j], i, j, input);
                    }

                    // updating result
                    if (COUNT >= current_max)
                    {
                        current_max = COUNT;
                        bestColor = input[i][j].GetComponent<Avocado>().color;
                        reset_result(input[i][j].GetComponent<Avocado>().color, input);
                    }
                } 
                
            }
        }
        int count = 0;
        Debug.Log("The largest connected match at these points:");
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (result[i][j] == 1)
                {
                    input[i][j].GetComponent<Avocado>().isPartOfMatch = true;
                    Debug.Log("At " + i + "," + j);
                    currentMatch[count] = input[i][j];
                    count++;
                }
            }
        }
        Debug.Log("The largest connected match of the grid is :" + current_max);
        Debug.Log(bestColor);

    }

    public static int[][] ReturnRectangularIntArray(int size1, int size2)
    {
        int[][] newArray = new int[size1][];
        for (int array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new int[size2];
        }

        return newArray;
    }
}


