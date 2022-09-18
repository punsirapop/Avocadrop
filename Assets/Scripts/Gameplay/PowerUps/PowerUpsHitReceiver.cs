using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsHitReceiver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SendMessageUpwards("Enter", collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SendMessageUpwards("Exit", collision, SendMessageOptions.DontRequireReceiver);
    }
}
