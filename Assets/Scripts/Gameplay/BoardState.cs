using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardState : MonoBehaviour
{
    [SerializeField] GameObject AvoCollection, MultiplierCollection, multiplier;

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
    static readonly int[][] resultForPatternSearch = ReturnRectangularIntArray(n+2, m+2);

    // stores the count of cells in the
    // largest connected component

    GameObject[][] gameState;
    public static int currentMatchCount;
    public static GameObject[] currentMatch = new GameObject[n*m];
    public static matchPattern currentPattern;
    public static int rainbowLeftToDrop = 0;
    public static int currentScore = 0;
    public static int rotationSinceLastMatch = 0;
    public static int xExplodeLeftTodo = 0;
    public static int plusExplodeLeftTodo = 0;
    public static bool isRerolling = false;



    public static float timeToGo;
    float rerollInterval = 10f;

    public enum matchPattern
    {
        square,
        cross,
        L_shape1,
        L_shape2,
        L_shape3,
        L_shape4,
        T_shape1,
        T_shape2,
        T_shape3,
        T_shape4,
        T_shape,
        none
    }
    
    Dictionary<matchPattern, int> patternSizeDictionary = new Dictionary<matchPattern, int>()
        {
            {matchPattern.cross,5},
            {matchPattern.L_shape1,5},
            {matchPattern.L_shape2,5},
            {matchPattern.L_shape3,5},
            {matchPattern.L_shape4,5},
            {matchPattern.T_shape1,5},
            {matchPattern.T_shape2,5},
            {matchPattern.T_shape3,5},
            {matchPattern.T_shape4,5},
            {matchPattern.square,4},
        };

    static readonly int[][] sqarePattern = createPattern(new int[4], new int[4] {0, 1, 1, 0}, new int[4] {0, 1, 1 ,0}, new int[4]);
    static readonly int[][] crossPattern = createPattern(new int[5], new int[5] {0,0,1,0,0}, new int[5] {0, 1, 1, 1 ,0}, new int[5] {0, 0, 1, 0 ,0 }, new int[5]);
    static readonly int[][] L_shapePattern1 = createPattern(new int[5], new int[5] {0, 1, 0, 0, 0 }, new int[5] {0, 1, 0, 0 , 0}, new int[5] {0, 1, 1, 1 ,0}, new int[5]);
    static readonly int[][] L_shapePattern2 = createPattern(new int[5], new int[5] {0, 0, 0, 1, 0 }, new int[5] {0, 0, 0, 1 , 0}, new int[5] {0, 1, 1, 1 ,0}, new int[5]);
    static readonly int[][] L_shapePattern3 = createPattern(new int[5], new int[5] {0, 1, 1, 1, 0 }, new int[5] {0, 0, 0, 1 , 0}, new int[5] {0, 0, 0, 1 ,0}, new int[5]);
    static readonly int[][] L_shapePattern4 = createPattern(new int[5], new int[5] {0, 1, 1, 1, 0 }, new int[5] {0, 1, 0, 0 , 0}, new int[5] {0, 1, 0, 0 ,0}, new int[5]);
    static readonly int[][] T_shapePattern1 = createPattern(new int[5], new int[5] {0, 1, 1, 1, 0 }, new int[5] {0, 0, 1, 0 , 0}, new int[5] {0, 0, 1, 0 ,0}, new int[5]);
    static readonly int[][] T_shapePattern2 = createPattern(new int[5], new int[5] {0, 0, 0, 1, 0 }, new int[5] {0, 1, 1, 1 , 0}, new int[5] {0, 0, 0, 1 ,0}, new int[5]);
    static readonly int[][] T_shapePattern3 = createPattern(new int[5], new int[5] {0, 0, 1, 0, 0 }, new int[5] {0, 0, 1, 0 , 0}, new int[5] {0, 1, 1, 1 ,0}, new int[5]);
    static readonly int[][] T_shapePattern4 = createPattern(new int[5], new int[5] {0, 1, 0, 0, 0 }, new int[5] {0, 1, 1, 1 , 0}, new int[5] {0, 1, 0, 0 ,0}, new int[5]);

    Dictionary<matchPattern, int[][]> patternToMatch = new Dictionary<matchPattern, int[][]>()
        {
            {matchPattern.square,sqarePattern},
            {matchPattern.cross,crossPattern},
            {matchPattern.L_shape1,L_shapePattern1},
            {matchPattern.L_shape2,L_shapePattern2},
            {matchPattern.L_shape3,L_shapePattern3},
            {matchPattern.L_shape4,L_shapePattern4},
            {matchPattern.T_shape1,T_shapePattern1},
            {matchPattern.T_shape2,T_shapePattern2},
            {matchPattern.T_shape3,T_shapePattern3},
            {matchPattern.T_shape4,T_shapePattern4},
        };


    public static int[][] createPattern(params int[][] patternRows)
    {
        int numOfRows = patternRows.Length;
        int[][] newArray = new int[numOfRows][];

        for (int i = 0; i < numOfRows; i++)
        {
            newArray[i] = patternRows[i];
        }

        return newArray;
    }
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

    private void FixedUpdate()
    {
        if (!PhaseManager.Instance.isGameEnded && Time.fixedTime >= timeToGo &&
            PhaseManager.Instance.phase == Phase.PlayerAction)
        {
            PhaseManager.Instance.PhaseChange(Phase.UpdateState);
            rerollBoard();
            resetRerollTimer();
        }
    }

    void resetRerollTimer()
    {
        timeToGo = Time.fixedTime + rerollInterval;
    }

    void rerollBoard()
    {
        Debug.Log("Rerolled board");
        gameObject.GetComponent<Timer>().AddTime(-20f);
        isRerolling = true;
        PowerUpsManager.Instance.YeetusDeletus(2);
    }

    private void Start()
    {
        resetRerollTimer();
        if (Instance == null)
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


    public IEnumerator explodeAllIfCan()
    {
        checkForMatchesAndDetectPatterns();
        if (currentMatchCount >= 3)
        {
            // reset timer for reroll and lock penalty
            rotationSinceLastMatch = 0;
            resetRerollTimer();

            while (currentMatchCount >= 3)
            {
                int multiplier = calculateMultiplier();
                Debug.Log("Multiplier: "+multiplier);
                //explode this color match
                yield return new WaitForSeconds(.5f);
                // explode all from this match
                for (int i = 0; i < currentMatchCount; i++)
                {
                    if (currentMatch[i] != null)
                    {
                        currentMatch[i].GetComponent<Avocado>().isPartOfMatch = false;
                        currentMatch[i].GetComponent<Avocado>().DeleteMe();
                    }
                }
                giveScoreAndBonusForNumberOfMatch(multiplier);
                activateEffectForPattern();


                updateState();
                checkForMatchesAndDetectPatterns();

            }
            PhaseManager.Instance.PhaseChange(Phase.Drop);
        }
        else
        {
            PhaseManager.Instance.PhaseChange(Phase.PreAction);
        }
    }

    public int calculateMultiplier()
    {
        int currentMultiplier = 1;
        // check in match area all multiplier
        // get positions of matches
        for (int i = 0; i < currentMatchCount; i++)
        {
            if (currentMatch[i] != null)
            {
                Vector2 posToCheck = currentMatch[i].GetComponent<Transform>().position;
                Debug.Log("Multiplier layer: " + LayerMask.GetMask("Multiplier"));
                Collider2D multiplierFound = Physics2D.OverlapCircle(posToCheck, 0.1f, LayerMask.GetMask("Multiplier"));

                if (multiplierFound)
                {
                    Debug.Log("Applying Multiplier.............................................................................................");
                    currentMultiplier *= multiplierFound.gameObject.GetComponent<Multiplier>().multiplierAmount;
                    // destroy used multiplier
                    Destroy(multiplierFound.gameObject);

                }
            }
        }
         return currentMultiplier;
    }

    public void giveRainbow(int amount)
    {
        rainbowLeftToDrop += amount;
    }
    public void giveManualPowerUp()
    {
        int randomPowerIndex = UnityEngine.Random.Range(0,5);
        PowerUpsManager.Instance.getPowerUp(randomPowerIndex);
    }

    public void generateMultiplier(int round)
    {
        List<Vector2> validPos = getNonObstaclePos();
        if (validPos.Count == 0) return;
        for (int i = 0; i < round; i++)
        {
            int random = UnityEngine.Random.Range(0, validPos.Count);
            Vector2 pos = validPos[random];
            GameObject mul = Instantiate(multiplier, MultiplierCollection.transform);
            validPos.RemoveAt(random);
            mul.transform.position = pos;
            int randomAmount = UnityEngine.Random.Range(2, 5);
            mul.GetComponent<Multiplier>().setMultiplierAmount(randomAmount);
        }
    }

    public List<Vector2> getNonObstaclePos()
    {
        List<Vector2> validPos = new List<Vector2>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Debug.Log("--------------------------------------------------------------------------------------");
                Debug.Log(i + "," + j);


                Collider2D obstacleFound = Physics2D.OverlapCircle(new Vector2(i, j), 0.4f, LayerMask.GetMask("Obstacle"));
                if (!obstacleFound)
                {
                    Collider2D multiplierFound = Physics2D.OverlapCircle(new Vector2(i, j), 0.4f, LayerMask.GetMask("Multiplier"));

                    if (!multiplierFound)
                    {
                        Debug.Log("Found valid pos at " + i + "," + j);
                        validPos.Add(new Vector2(i, j));
                    }
                    
                }

            }
        }
        return validPos;
    }

    public void giveScoreAndBonusForNumberOfMatch(int multiplier)
    {
        int score = 0;
        if (currentMatchCount == 3)
        {
            score = 30;
        }
        else if (currentMatchCount == 4)
        {
            score = 50;
            giveRainbow(1);
        }
        else if (currentMatchCount >= 5)
        {
            score = 100 + (currentMatchCount - 5)*100;
            giveRainbow(currentMatchCount - 5);
            giveManualPowerUp();
        }

        gameObject.GetComponent<Timer>().AddTime(1f);
        currentScore += (score*multiplier);
    }

    public void activateEffectForPattern()
    {
        if (currentPattern == matchPattern.square)
        {
            gameObject.GetComponent<Timer>().AddTime(15f);
        }
        else if (currentPattern == matchPattern.cross)
        {
            plusExplodeLeftTodo++;
        }
        else if (currentPattern == matchPattern.L_shape1 || currentPattern == matchPattern.L_shape2 || currentPattern == matchPattern.L_shape3 || currentPattern == matchPattern.L_shape4)
        {
            xExplodeLeftTodo++;
        }
        else if (currentPattern == matchPattern.T_shape1 || currentPattern == matchPattern.T_shape2 || currentPattern == matchPattern.T_shape3 || currentPattern == matchPattern.T_shape4)
        {
            TshapeEffect();
            
        }
    }

    public void TshapeEffect()
    {
        generateMultiplier(4);

    }

    public void updateState()
    {
        int count = 0;

        checkLock();

        //reset arrays
        gameState = ReturnRectangularGameObjectArray(9, 9);
        currentMatch = new GameObject[n*m];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {

                Collider2D avocadoFound = Physics2D.OverlapCircle(new Vector2(i, j), 0.1f, LayerMask.GetMask("Avocado"));

                if (avocadoFound)
                {
                    count++;
                    gameState[i][j] = avocadoFound.gameObject;
                }

            }
        }


    }

    public void checkLock()
    {
        int rotationUntilLock = 5;
        if (rotationSinceLastMatch < rotationUntilLock) return;

        bool found = false;
        Avocado[] avos =  AvoCollection.GetComponentsInChildren<Avocado>();

        while (!found)
        {
            int random = UnityEngine.Random.Range(0, avos.Length);
            if (avos[random].gameObject.layer == 8)
            {
                found = true;
                avos[random].lockMe();
                rotationSinceLastMatch = 0;
            }
        }
    }

    public void checkForMatchesAndDetectPatterns()
    {
        computeLargestConnectedGrid(gameState);
        currentPattern = detectWhatPattern();
        Debug.Log("Found this pattern: " + currentPattern);
    }

    void makeArrayForPatternSearch()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                resultForPatternSearch[i + 1][j + 1] = result[i][j];
            }
        }
    }

    matchPattern detectWhatPattern()
    {
        makeArrayForPatternSearch();
        foreach (matchPattern pattern in patternSizeDictionary.Keys)
        {
            if (isThisPattern(pattern))
            {
                return pattern;
            }
        }

        return matchPattern.none;


    }

    bool isThisPattern(matchPattern pattern)
    {
        for (int i = 0; i <= n+2 - patternSizeDictionary[pattern]; i++)
        {
            for (int j = 0; j <= m+2 - patternSizeDictionary[pattern]; j++)
            {
                if (checkIfPatternAt(pattern,i, j))
                {
                    return true;
                }
            }
                
        }

        return false;
    }

    bool checkIfPatternAt(matchPattern pattern,int i, int j)
    {
        int[][] patternTemplate = patternToMatch[pattern];
        for (int x = 0; x < patternSizeDictionary[pattern]; x++)
        {
            for (int y = 0; y < patternSizeDictionary[pattern]; y++)
            {
                if (resultForPatternSearch[i+x][j+y] != patternTemplate[x][y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Function checks if a cell is valid i.e
    // it is inside the grid and equal to the key
    internal static bool is_valid(int x, int y, Color key, GameObject[][] input)
    {
        if (x < n && y < m && x >= 0 && y >= 0)
        {
            if (input[x][y] != null)
            {
                if (visited[x][y] == 0 && (input[x][y].GetComponent<Avocado>().color.Equals(key) || input[x][y].GetComponent<Avocado>().colorEnum.Equals(Avocado.colorText.rainbow)))
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

    internal static bool noObstacle(int i, int j, Vector2 dir)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(i,j), dir, 1f, LayerMask.GetMask("Obstacle"));
        if (hits.Length == 1 && hits[0].collider.gameObject.layer == 9)
        {
            return false;
        }
        return true;
    }

    // BFS to find all cells in
    // connection with key = input[i][j]
    public static void BFS(GameObject x, GameObject y, int i, int j, GameObject[][] input, Vector2 dir)
    {
        // terminating case for BFS
        if (input[i][j] == null || x == null || y == null || !(x.GetComponent<Avocado>().color.Equals(y.GetComponent<Avocado>().color) || x.GetComponent<Avocado>().colorEnum.Equals(Avocado.colorText.rainbow) || y.GetComponent<Avocado>().colorEnum.Equals(Avocado.colorText.rainbow)))
        {
            return;
        }

        visited[i][j] = 1;
        currentMatchCount++;

        // x_move and y_move arrays
        // are the possible movements
        // in x or y direction
        int[] x_move = new int[] { 0, 0, 1, -1 };
        int[] y_move = new int[] { 1, -1, 0, 0 };

        // checks all four points
        // connected with input[i][j]
        for (int u = 0; u < 4; u++)
        {
            Vector2 nextDir = new Vector2(y_move[u], x_move[u]);
            if (is_valid(i + y_move[u], j + x_move[u], x.GetComponent<Avocado>().color, input) && noObstacle(i,j,nextDir))
            {
                BFS(input[i][j], input[i + y_move[u]][j + x_move[u]], i + y_move[u], j + x_move[u], input, nextDir);
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
                if (visited[i][j] == 1 && (input[i][j].GetComponent<Avocado>().color.Equals(key) || input[i][j].GetComponent<Avocado>().colorEnum.Equals(Avocado.colorText.rainbow)))
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
        List<Avocado.colorText> colorList = new List<Avocado.colorText> { Avocado.colorText.green, Avocado.colorText.red, Avocado.colorText.yellow, Avocado.colorText.blue, Avocado.colorText.magenta, Avocado.colorText.cyan };

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (input[i][j] != null)
                {
                    if (input[i][j].GetComponent<Avocado>().colorEnum.Equals(Avocado.colorText.rainbow))
                    {
                        foreach (Avocado.colorText colorToCheck in colorList)
                        {
                            input[i][j].GetComponent<Avocado>().applyColor(colorToCheck);

                            reset_visited();
                            currentMatchCount = 0;

                            // checking cell to the right
                            if (j + 1 < m)
                            {
                                BFS(input[i][j], input[i][j + 1], i, j, input, new Vector2(0, 1));
                            }

                            // updating result
                            if (currentMatchCount >= current_max)
                            {
                                current_max = currentMatchCount;
                                bestColor = input[i][j].GetComponent<Avocado>().color;
                                reset_result(input[i][j].GetComponent<Avocado>().color, input);
                            }
                            reset_visited();
                            currentMatchCount = 0;

                            // checking cell downwards
                            if (i + 1 < n)
                            {
                                BFS(input[i][j], input[i + 1][j], i, j, input, new Vector2(1, 0));
                            }

                            // updating result
                            if (currentMatchCount >= current_max)
                            {
                                current_max = currentMatchCount;
                                bestColor = input[i][j].GetComponent<Avocado>().color;
                                reset_result(input[i][j].GetComponent<Avocado>().color, input);
                            }
                        }
                    }
                    else
                    {
                        reset_visited();
                        currentMatchCount = 0;

                        // checking cell to the right
                        if (j + 1 < m)
                        {
                            BFS(input[i][j], input[i][j + 1], i, j, input, new Vector2(0, 1));
                        }

                        // updating result
                        if (currentMatchCount >= current_max)
                        {
                            current_max = currentMatchCount;
                            bestColor = input[i][j].GetComponent<Avocado>().color;
                            reset_result(input[i][j].GetComponent<Avocado>().color, input);
                        }
                        reset_visited();
                        currentMatchCount = 0;

                        // checking cell downwards
                        if (i + 1 < n)
                        {
                            BFS(input[i][j], input[i + 1][j], i, j, input, new Vector2(1, 0));
                        }

                        // updating result
                        if (currentMatchCount >= current_max)
                        {
                            current_max = currentMatchCount;
                            bestColor = input[i][j].GetComponent<Avocado>().color;
                            reset_result(input[i][j].GetComponent<Avocado>().color, input);
                        }
                    }
                        
                } 
                
            }
        }
        int count = 0;
        if (current_max > 2)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (result[i][j] == 1)
                    {
                        input[i][j].GetComponent<Avocado>().isPartOfMatch = true;
                        currentMatch[count] = input[i][j];
                        count++;
                    }
                }
            }
        }
        currentMatchCount = current_max;

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


