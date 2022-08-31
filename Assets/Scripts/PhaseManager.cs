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

    [SerializeField] GameObject AvoCollection;
    [SerializeField] Transform Environment;
    [SerializeField] TextMeshProUGUI currentPhaseDisplay, dropCountDisplay;

    Dictionary<Transform, int> avoDict = new Dictionary<Transform, int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Debug.Log("Changing Phase from Start");
        PhaseChange(Phase.Preparation);
    }

    private void Update()
    {
        currentPhaseDisplay.SetText("Current Phase: " + phase);
        dropCountDisplay.SetText("Avocado Count: " + AvoCollection.transform.childCount);
        if(phase == Phase.PlayerAction)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Debug.Log("Changing Phase from PlayerAction");
                StartCoroutine(RotateAndDrop(90f));
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Debug.Log("Changing Phase from PlayerAction");
                StartCoroutine(RotateAndDrop(-90f));
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Debug.Log("Changing Phase from PlayerAction");
                StartCoroutine(RotateAndDrop(180f));
            }
        }
    }

    public void PhaseChange(Phase newPhase)
    {
        phase = newPhase;

        switch (phase)
        {
            case Phase.Preparation:
                break;
            case Phase.PlayerAction:
                HandlePlayerAction();
                break;
            case Phase.CheckExplode:
                HandleCheckExplode();
                break;
            case Phase.Explode:
                break;
            case Phase.Drop:
                StartCoroutine(HandleDrop());
                break;
            case Phase.Spawn:
                break;
        }
        
        OnPhaseChanged?.Invoke(newPhase);
        Debug.Log("=========== Current Phase: " + phase + " ===========");
    }

    private void HandlePlayerAction()
    {
        
    }

    private void HandleCheckExplode()
    {

    }

    private IEnumerator RotateAndDrop(float angle)
    {
        Environment.rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
        /*
        Quaternion rotateTo = Environment.rotation * Quaternion.AngleAxis(angle, Vector3.forward);
        while (Quaternion.Angle(Environment.rotation, rotateTo) > 0)
        {
            Environment.rotation = Quaternion.Slerp(Environment.rotation, rotateTo, Time.deltaTime);
        }
        */
        yield return new WaitForFixedUpdate();
        PhaseChange(Phase.Drop);
    }

    private IEnumerator HandleDrop()
    {
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
    Explode,
    Drop,
    Spawn
}
