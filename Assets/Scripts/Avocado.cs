using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avocado : MonoBehaviour
{
    public Color color;
    public bool pleaseDrop = false;

    GameObject fallingPoint;
    Transform pointCollection;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        // PhaseManager.OnPhaseChanged += HandlePhaseChanged;

        pointCollection = GameObject.Find("PointCollection").transform;

        fallingPoint = new GameObject("fallingPoint");
        fallingPoint.transform.position = transform.position;
        fallingPoint.transform.parent = pointCollection;

        color = spriteRenderer.color;
    }

    private void OnDisable()
    {
        // PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void Start()
    {
        /*
        pointCollection = GameObject.Find("PointCollection").transform;

        fallingPoint = new GameObject("fallingPoint");
        fallingPoint.transform.position = transform.position;
        fallingPoint.transform.parent = pointCollection;

        color = spriteRenderer.color;
        */
        fallingPoint.transform.position = transform.position;
    }

    private void Update()
    {
        if (pleaseDrop)
        {
            spriteRenderer.color = Color.black;
            Drop();
        }
        else
        {
            spriteRenderer.color = color;
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, fallingPoint.transform.position, 3f);
    }

    /*
    private void HandlePhaseChanged(Phase newPhase)
    {
        isDropped = (newPhase == Phase.Drop);
    }
    */

    public void PleaseDrop()
    {
        pleaseDrop = true;
    }

    public void Drop()
    {
        // Debug.Log("Dropping...");
        int layermask = ~(LayerMask.GetMask("Avocado")) & ~(LayerMask.GetMask("Grid"));
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1f, -layermask);

        if (hits.Length == 1)
        {
            fallingPoint.transform.position = hits[0].transform.position;
        }
        else
        {
            // PhaseManager.Instance.SendMessage("CountDrop", transform);
            // Debug.Log("Sent msg from " + transform.position);
            PhaseManager.isDropping = false;
            pleaseDrop = false;
        }
    }

    /*
    private void Update()
    {
        if (phase == Phase.Drop) DropControl(gameObject);

        if (isDropped)
        {
            spriteRenderer.color = Color.black;
        }
        else
        {
            spriteRenderer.color = color;
        }
    }
    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, fallingPoint.transform.position, 1f);
    }

    void HandlePhaseChanged(Phase newPhase)
    {
        phase = newPhase;

        isDropped = !(phase == Phase.Drop);
        // Debug.Log("Phase changed to " + newPhase + " from " + transform.position);
    }

    public void DropControl(GameObject avo)
    {
        int layermask = ~(LayerMask.GetMask("Avocado")) & ~(LayerMask.GetMask("Grid"));
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1f, -layermask);

        if (avo != gameObject && !avo.GetComponent<Avocado>().isDropped)
        {
            isDropped = false;
        }

        switch (hits.Length)
        {
            case 0:
                if (!isDropped)
                {
                    PhaseManager.Instance.SendMessage("CountDrop", transform);
                    Debug.Log("Sent msg from " + transform.position);
                    isDropped = true;
                }
                break;
            case 1:
                fallingPoint.transform.position = hits[0].transform.position;
                break;
            case 2:
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.layer == 8)
                    {
                        hit.collider.SendMessage("DropControl", avo);
                        isDropped = true;
                    }
                }
                break;
        }
    }
    */

    /*
    public void DropControl(GameObject avo)
    {
        int layermask = ~(LayerMask.GetMask("Avocado")) & ~(LayerMask.GetMask("Grid"));
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1f, -layermask);

        if(avo != gameObject && !avo.GetComponent<Avocado>().isDropped)
        {
            isDropped = false;
        }

        switch (hits.Length)
        {
            case 0:
                if (!isDropped)
                {
                    PhaseManager.Instance.SendMessage("CountDrop");
                    Debug.Log("Sent msg from " + transform.position);
                    isDropped = true;
                }
                break;
            case 1:
                fallingPoint.transform.position = hits[0].transform.position;
                break;
            case 2:
                foreach(RaycastHit2D hit in hits)
                {
                    if(hit.collider.gameObject.layer == 8)
                    {
                        hit.collider.SendMessage("DropControl", avo);
                        isDropped = true;
                    }
                }
                break;
        }
    }
    */

    /*
    void Drop(Vector3 destination)
    {
        if (Vector2.Distance(transform.position, fallingPoint.transform.position) < .5f)
        {
            int layermask = ~(LayerMask.GetMask("Avocado")) & ~(LayerMask.GetMask("Grid"));
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1f, -layermask);
        
            if(hits.Length == 1 && !isDropped)
            {
                fallingPoint.transform.position = hits[0].transform.position;
            }
            else
            {
                phaseManager.SendMessage("CountDrop", gameObject);
                isDropped = true;
            }
        }
    }
    */
}