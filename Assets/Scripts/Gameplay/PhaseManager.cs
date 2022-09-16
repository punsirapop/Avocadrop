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

    [SerializeField] GameObject AvoCollection;
    [SerializeField] Transform Environment;
    [SerializeField] TextMeshProUGUI currentPhaseDisplay, dropCountDisplay, patternFoundDisplay;

    Dictionary<Transform, int> avoDict = new Dictionary<Transform, int>();
    bool isGameEnded = false;
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
        PhaseChange(Phase.Preparation);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        currentPhaseDisplay.SetText("Current Phase: " + phase);
        dropCountDisplay.SetText("Avocado Count: " + AvoCollection.transform.childCount);
        patternFoundDisplay.SetText("Pattern Found: " + BoardState.Instance.currentPattern);

        if (phase == Phase.PlayerAction)
        {
            if (isGameEnded)
            {
                PhaseChange(Phase.GameEnd);
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                // Debug.Log("Changing Phase from PlayerAction");
                StartCoroutine(RotateAndDrop(90f));
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                // Debug.Log("Changing Phase from PlayerAction");
                StartCoroutine(RotateAndDrop(-90f));
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                // Debug.Log("Changing Phase from PlayerAction");
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
            case Phase.Revealing:
                HandleRevealing();
                break;
            default:
                break;
        }
        
        OnPhaseChanged?.Invoke(newPhase);
    }

    private void HandleRevealing()
    {
        MazeSpawner.Instance.Reveal(5 * revealRequest);
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
        yield return new WaitUntil(() => doneDropCount == SpawnManager.Instance.capacity);
        BoardState.Instance.updateState();
        PhaseChange(Phase.CheckExplode);
    }

    private void HandleGameEnd()
    {
        isGameEnded = true;
    }

    public void EndMe()
    {
        isGameEnded = true;
    }

    public void RevealRequest()
    {
        revealRequest++;
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
    Revealing
}
