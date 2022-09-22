using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rerolling : MonoBehaviour
{
    private void OnEnable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void HandlePhaseChanged(Phase phase)
    {
        if (phase == Phase.PlayerAction)
        {
            Debug.Log("REROLL TEXT OFF");
            gameObject.SetActive(false);
        }
    }
}
