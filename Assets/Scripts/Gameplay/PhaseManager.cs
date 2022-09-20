using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance;
    public static event Action<Phase> OnPhaseChanged;
    public bool isDropping = false;
    public Phase phase;
    public int doneDropCount = 0;
    public bool isGameEnded = false;
    public bool isPaused = false;

    [SerializeField] GameObject AvoCollection, PauseLid, GameCanvas, EndCanvas;
    [SerializeField] Transform Environment;
    [SerializeField] TextMeshProUGUI currentPhaseDisplay, dropCountDisplay, patternFoundDisplay, scoreDisplay;

    Dictionary<Transform, int> avoDict = new Dictionary<Transform, int>();
    int revealRequest = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Debug.Log("Changing Phase from Start");
        BoardState.currentScore = 0;
        PhaseChange(Phase.Preparation);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (isGameEnded)
        {
            PhaseChange(Phase.GameEnd);
        }

        currentPhaseDisplay.SetText("Current Phase: " + phase);
        // dropCountDisplay.SetText("Avocado Count: " + AvoCollection.transform.childCount);
        // patternFoundDisplay.SetText("Pattern Found: " + BoardState.currentPattern);
        scoreDisplay.SetText(BoardState.currentScore.ToString());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (phase == Phase.PlayerAction && !isPaused)
        {
            if (BoardState.xExplodeLeftTodo>0)
            {
                BoardState.xExplodeLeftTodo--;
                PowerUpsManager.Instance.YeetusDeletus(1);
            }
            else if (BoardState.plusExplodeLeftTodo > 0)
            {
                BoardState.plusExplodeLeftTodo--;
                PowerUpsManager.Instance.YeetusDeletus(0);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                BoardState.rotationSinceLastMatch++;
                // Debug.Log("Changing Phase from PlayerAction");
                StartCoroutine(RotateAndDrop(90f));
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                // Debug.Log("Changing Phase from PlayerAction");
                BoardState.rotationSinceLastMatch++;
                StartCoroutine(RotateAndDrop(-90f));
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                // Debug.Log("Changing Phase from PlayerAction");
                BoardState.rotationSinceLastMatch++;
                StartCoroutine(RotateAndDrop(180f));
            }
        }
    }

    public void PhaseChange(Phase newPhase)
    {
        Debug.Log("=========== From Phase: "+ phase + " to Current Phase: " + newPhase + " ===========");
        phase = newPhase;

        switch (phase)
        {
            case Phase.Preparation:
                break;
            case Phase.PlayerAction:
                break;
            case Phase.CheckExplode:
                HandleCheckExplode();
                break;
            case Phase.Drop:
                StartCoroutine(HandleDrop());
                break;
            case Phase.Spawn:
                break;
            case Phase.UpdateState:
                StartCoroutine(HandleUpdateState());
                break;
            case Phase.GameEnd:
                HandleGameEnd();
                break;
            case Phase.PreAction:
                HandlePreAction();
                break;
            default:
                break;
        }
        
        OnPhaseChanged?.Invoke(newPhase);
    }

    private void HandlePreAction()
    {
        MazeSpawner.Instance.Reveal(revealRequest-MazeSpawner.Instance.revealedSoFar);
        PhaseChange(Phase.PlayerAction);
    }

    private void HandleCheckExplode()
    {
        Debug.Log("In handle check explode");
        StartCoroutine(BoardState.Instance.explodeAllIfCan());
    }

    private IEnumerator RotateAndDrop(float angle)
    {
        //Environment.rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
        PhaseChange(Phase.rotating);
        float rotationSpeed = 1000f;
        Quaternion rotateTo = Environment.rotation * Quaternion.AngleAxis(angle, Vector3.forward);

        while (Quaternion.Angle(Environment.rotation, rotateTo) > 0)
        {
            float newAngle = Quaternion.Angle(Environment.rotation, rotateTo);
            float timeToComplete = newAngle / rotationSpeed;
            float donePercentage = Mathf.Min(1F, Time.deltaTime / timeToComplete);
            Environment.rotation = Quaternion.Slerp(Environment.rotation, rotateTo, donePercentage);
            Debug.Log(donePercentage);
            yield return new WaitForFixedUpdate();
        }

        PhaseChange(Phase.Drop);
    }

    private IEnumerator HandleDrop()
    {
        doneDropCount = 0;
        avoDict.Clear();

        foreach (Transform avo in AvoCollection.transform)
        {
            int height = Mathf.RoundToInt(avo.position.y);
            avoDict.Add(avo, height);
        }

        int oldHeight = -99;
        foreach (KeyValuePair<Transform, int> sortAvo in avoDict.OrderBy(key => key.Value))
        {
            if(oldHeight == -99) oldHeight = sortAvo.Value;
            // yield return new WaitWhile(() => isDropping);

            
            if(sortAvo.Value > oldHeight)
            {
                yield return new WaitWhile(() => isDropping);
            }
            

            isDropping = true;
            sortAvo.Key.gameObject.SendMessage("PleaseDrop");
            oldHeight = sortAvo.Value;
        }

        yield return new WaitWhile(() => isDropping);

        PhaseChange(Phase.Spawn);
    }

    private IEnumerator HandleUpdateState()
    {
        PauseLid.SetActive(false);
        yield return new WaitUntil(() => doneDropCount == SpawnManager.Instance.capacity);
        BoardState.Instance.updateState();
        PhaseChange(Phase.CheckExplode);
    }

    private void HandleGameEnd()
    {
        Debug.Log("HANDLING THE END");
        PauseLid.SetActive(true);
        GameCanvas.SetActive(false);
        EndCanvas.SetActive(true);
    }

    public void EndMe()
    {
        Debug.Log("Initiate Summarization Sequence");
        isGameEnded = true;
    }

    public void RevealRequest()
    {
        revealRequest++;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        PauseLid.SetActive(isPaused);
    }

    /*
    public void CountDrop(Transform place)
    {
        Debug.Log("Got msg from " + place.position);
        avoDict.Remove(place);
        Debug.Log("Count: " + avoDict.Count + "/" +
            AvoCollection.transform.childCount + " - " + place.position);

        if (avoDict.Count == 0)
        {
            Debug.Log("Changing Phase from CountDrop");
            PhaseChange(Phase.Spawn);
        }
    }
    */
}

public enum Phase
{
    Preparation,
    PlayerAction,
    CheckExplode,
    Drop,
    Spawn,
    UpdateState,
    GameEnd,
    rotating,
    PreAction
}
