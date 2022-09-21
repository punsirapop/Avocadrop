using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    Button me;

    private void Awake()
    {
        me = GetComponent<Button>();
        PhaseManager.OnPhaseChanged += InactivateMe;
    }

    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= InactivateMe;
    }

    private void InactivateMe(Phase phase)
    {
        if(phase == Phase.GameEnd) me.interactable = false;
    }
}
