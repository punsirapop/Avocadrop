using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance;
    public static event Action<Phase> OnPhaseChanged;
    public Phase phase;

    [SerializeField] GameObject AvoCollection;
    [SerializeField] Transform Environment;

    int dropCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhaseChange(Phase.Spawn);
    }

    private void Update()
    {
        if(phase == Phase.PlayerAction)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Environment.rotation = Quaternion.AngleAxis(-90f, Vector3.forward);
                PhaseChange(Phase.Drop);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Environment.rotation = Quaternion.AngleAxis(90f, Vector3.forward);
                PhaseChange(Phase.Drop);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Environment.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
                PhaseChange(Phase.Drop);
            }
        }
    }

    public void PhaseChange(Phase newPhase)
    {
        Debug.Log("Current Phase: " + newPhase);
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
            PhaseChange(Phase.Spawn);
        }
    }

    public void CountDrop(GameObject avo)
    {
        dropCount++;

        Debug.Log(dropCount + "/" + AvoCollection.transform.childCount);
        if (dropCount == AvoCollection.transform.childCount)
        {
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
