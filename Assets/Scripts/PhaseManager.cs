using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance;
    public static event Action<Phase> OnPhaseChanged;
    public Phase phase;

    [SerializeField] GameObject AvoCollection;
    [SerializeField] Transform Environment;
    [SerializeField] TextMeshProUGUI currentPhaseDisplay, dropCountDisplay;

    int dropCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("Changing Phase from Start");
        PhaseChange(Phase.Spawn);
    }

    private void Update()
    {
        currentPhaseDisplay.SetText("Current Phase: " + phase);
        dropCountDisplay.SetText("Dropped: " + dropCount + "/" + AvoCollection.transform.childCount);
        if(phase == Phase.PlayerAction)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Debug.Log("Changing Phase from PlayerAction");
                Environment.rotation *= Quaternion.AngleAxis(-90f, Vector3.forward);
                PhaseChange(Phase.Drop);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Debug.Log("Changing Phase from PlayerAction");
                Environment.rotation *= Quaternion.AngleAxis(90f, Vector3.forward);
                PhaseChange(Phase.Drop);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Debug.Log("Changing Phase from PlayerAction");
                Environment.rotation *= Quaternion.AngleAxis(180f, Vector3.forward);
                PhaseChange(Phase.Drop);
            }
        }
    }

    public void PhaseChange(Phase newPhase)
    {
        phase = newPhase;

        switch (phase)
        {
            case Phase.PlayerAction:
                HandlePlayerAction();
                break;
            case Phase.CheckExplode:
                HandleCheckExplode();
                break;
            case Phase.Explode:
                break;
            case Phase.Drop:
                HandleDrop();
                break;
            case Phase.Spawn:
                break;
        }

        Debug.Log("=========== Current Phase: " + phase + " ===========");
        OnPhaseChanged?.Invoke(newPhase);
    }

    private void HandlePlayerAction()
    {
        
    }

    private void HandleCheckExplode()
    {

    }

    private void HandleDrop()
    {
        dropCount = 0;

        if(AvoCollection.transform.childCount == 0)
        {
            Debug.Log("Changing Phase from HandleDrop");
            PhaseChange(Phase.Spawn);
        }
    }

    public void CountDrop()
    {
        dropCount++;
        Debug.Log("Count: " + dropCount + "/" + AvoCollection.transform.childCount);

        if (dropCount == AvoCollection.transform.childCount)
        {
            Debug.Log("Changing Phase from CountDrop");
            PhaseChange(Phase.Spawn);
        }
    }
}

public enum Phase
{
    PlayerAction,
    CheckExplode,
    Explode,
    Drop,
    Spawn
}
