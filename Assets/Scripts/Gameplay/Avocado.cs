using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Avocado : MonoBehaviour, IPointerClickHandler
{
    public Color color;
    public bool pleaseDrop = false, isPartOfMatch;

    GameObject fallingPoint;
    Transform pointCollection;
    colorText colorEnum;

    [SerializeField] SpriteRenderer spriteRenderer;

    public enum colorText
    {
        green,
        red,
        yellow,
        blue,
        magenta,
        cyan,
    }

    private void OnEnable()
    {
        // PhaseManager.OnPhaseChanged += HandlePhaseChanged;

        //pointCollection = GameObject.Find("PointCollection").transform;

        //fallingPoint = new GameObject("fallingPoint");
        //fallingPoint.layer = LayerMask.NameToLayer("fallingPoint");
        //fallingPoint.transform.position = transform.position;
        //fallingPoint.transform.parent = pointCollection;

        //color = spriteRenderer.color;
        colorEnum = randomColor();
        applyColor(colorEnum);
    }

    private void OnDisable()
    {
        // PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private colorText randomColor()
    {
        List<colorText> colorList = new List<colorText> { colorText.green, colorText.red, colorText.yellow, colorText.blue, colorText.magenta, colorText.cyan };
        int index = Random.Range(0, colorList.Count);
        //Debug.Log(colorList[index]);
        return colorList[index];
    }

    private void applyColor(colorText colorEnum)
    {
        switch (colorEnum)
        {
            case colorText.green:
                color = Color.green;
                break;
            case colorText.red:
                color = Color.red;
                break;
            case colorText.yellow:
                color = Color.yellow;
                break;
            case colorText.blue:
                color = Color.blue;
                break;
            case colorText.magenta:
                color = Color.magenta;
                break;
            case colorText.cyan:
                color = Color.cyan;
                break;
        }

        spriteRenderer.color = color;
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
        //fallingPoint.transform.position = transform.position;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PhaseManager.Instance.phase == Phase.PlayerAction)
        {
            SpawnManager.Instance.SendMessage("Despawn", gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (pleaseDrop)
        {
            //spriteRenderer.color = Color.black;
            Drop();
        }
        /*
        else
        {
            spriteRenderer.color = color;
        }
        */

        /*
        if(currentPhase == Phase.PlayerAction)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnManager.instance.SendMessage("Despawn", gameObject);
            }
        }
        */
    }

    private void LateUpdate()
    {
        //transform.position = Vector2.MoveTowards(transform.position, fallingPoint.transform.position, 3f);
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
        // int layermask = ~(LayerMask.GetMask("Avocado")) & ~(LayerMask.GetMask("Grid"));
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, .65f);

        if (hits.Length == 1 && hits[0].collider.gameObject.layer == 7)
        {
            //fallingPoint.transform.position = hits[0].transform.position;
            //transform.position = hits[0].transform.position;
            transform.position = Vector2.Lerp(transform.position, hits[0].transform.position, 3f);
        }
        else
        {
            // PhaseManager.Instance.SendMessage("CountDrop", transform);
            // Debug.Log("Sent msg from " + transform.position);
            PhaseManager.Instance.isDropping = false;
            pleaseDrop = false;
            Debug.Log("Done dropping");
            PhaseManager.Instance.doneDropCount++;
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