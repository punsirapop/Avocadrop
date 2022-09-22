using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOnPause : MonoBehaviour
{
    [SerializeField] GameObject[] toClose;
    private void Update()
    {
        if (PhaseManager.Instance.isPaused)
        {
            foreach(GameObject go in toClose)
            {
                go.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject go in toClose)
            {
                go.SetActive(true);
            }
        }
    }
}
