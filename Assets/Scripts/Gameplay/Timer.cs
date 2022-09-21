using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float timeCount = 0f;

    [SerializeField] TextMeshProUGUI timeDisplay;

    float timeRemaining = 40f;
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
            PhaseManager.Instance.InvokeRepeating("RevealRequest", 0f, 20f);
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
                timeCount += Time.deltaTime;
            }
            else
            {
                Debug.Log("Out of time!");
                PhaseManager.Instance.SendMessage("EndMe");
                isTimeRunning = false;
            }
        }
    }

    public void AddTime(float time)
    {
        timeRemaining += time;
    }
}
