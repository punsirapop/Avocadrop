using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsHitReceiver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhaseManager.Instance.isPaused) SendMessageUpwards("Enter", collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PhaseManager.Instance.isPaused) 
            SendMessageUpwards("Exit", collision, SendMessageOptions.DontRequireReceiver);
    }
}
