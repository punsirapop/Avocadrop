using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeDisplay;

    float timeRemaining = 10f;
    bool isTimeRunning = false, isPrepared = false;

    private void OnEnable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }
    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void HandlePhaseChanged(Phase phase)
    {
        if(phase == Phase.Preparation)
        {
            isPrepared = false;
        }

        if(phase == Phase.PlayerAction && !isPrepared)
        {
            isTimeRunning = true;
            isPrepared = true;
        }
    }

    private void Update()
    {
        float minute = Mathf.FloorToInt(timeRemaining / 60);
        float second = Mathf.FloorToInt(timeRemaining % 60);
        timeDisplay.text = string.Format("{0:00}:{1:00}", minute, second);

        if (isTimeRunning)
        {
            if(timeRemaining > 1)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Out of time!");
                isTimeRunning = false;
            }
        }
    }
}
