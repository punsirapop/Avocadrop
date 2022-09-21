using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float timeCount = 0f;

    [SerializeField] TextMeshProUGUI timeDisplay, hiddenTimeDisplay;

    float timeRemaining = 5f;
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
        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }
        float minute = Mathf.FloorToInt(timeRemaining / 60);
        float second = Mathf.FloorToInt(timeRemaining % 60);
        timeDisplay.text = string.Format("{0:00}:{1:00}", minute, second);
        float hMinute = Mathf.FloorToInt(timeCount / 60);
        float hSecond = Mathf.FloorToInt(timeCount % 60);
        hiddenTimeDisplay.text = "Time: " + string.Format("{0:00}:{1:00}", hMinute, hSecond);

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
