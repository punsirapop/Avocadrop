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
    public colorText colorEnum;
    int isLock = 0;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject lockSprite, permLockSprite;

    [SerializeField]
    Gradient gradient;
    float duration = 1.0f;
    float t = 0f;

    bool gradientIncreasing = true;

    public enum colorText
    {
        green,
        red,
        yellow,
        blue,
        magenta,
        cyan,
        rainbow,
    }

    private void OnEnable()
    {
        
        if (BoardState.rainbowLeftToDrop > 0)
        {
            colorEnum = colorText.rainbow;
            BoardState.rainbowLeftToDrop--;
        }
        else
        {
            colorEnum = randomColor();
            applyColor(colorEnum);
        }
    }

    private void OnDisable()
    {
        // PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private colorText randomColor()
    {
        List<colorText> colorList = new List<colorText> { colorText.green, colorText.red, colorText.yellow, colorText.blue, colorText.magenta, colorText.cyan };
        //List<colorText> colorList = new List<colorText> { colorText.green, colorText.red, colorText.yellow,};

        int index = Random.Range(0, colorList.Count);
        //Debug.Log(colorList[index]);
        return colorList[index];
    }

    public void applyColor(colorText colorEnum)
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

        //timeToGo = Time.fixedTime + rainbowColorSwitchInterval;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PhaseManager.Instance.phase == Phase.PlayerAction)
        {
            Debug.Log("CLICKED");
            isLock = (isLock + 1) % 3;

            lockSprite.SetActive(false);
            permLockSprite.SetActive(false);
            gameObject.layer = 8;
            switch (isLock)
            {
                case 0:
                    break;
                case 1:
                    lockSprite.SetActive(true);
                    break;
                case 2:
                    permLockSprite.SetActive(true);
                    gameObject.layer = 9;
                    break;
            }
            // DeleteMe();
            //checkObstacleNearMe();
        }
    }

    public void lockMe()
    {
        isLock = (isLock + 1) % 3;

        lockSprite.SetActive(false);
        permLockSprite.SetActive(false);
        gameObject.layer = 8;
        switch (isLock)
        {
            case 0:
                break;
            case 1:
                lockSprite.SetActive(true);
                break;
            case 2:
                permLockSprite.SetActive(true);
                gameObject.layer = 9;
                break;
        }
    }

    //test lasser
    public void checkObstacleNearMe()
    {
        int[] x_move = new int[] { 0, 0, 1, -1 };
        int[] y_move = new int[] { 1, -1, 0, 0 };

        // right -> left -> up -> down
        for (int u = 0; u < 4; u++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(y_move[u], x_move[u]), 1f, LayerMask.GetMask("Obstacle"));

            Debug.Log("--------------------------------------- laser hits ------------------------");
            Debug.Log("att: " + transform.position);
            Debug.Log("hits amount: " + hits.Length);
            Debug.Log("direction: " + new Vector2(y_move[u], x_move[u]));
            Debug.Log("hits.Length: " + hits.Length);
            Debug.Log(hits.Length == 1);
            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log(hit.collider.gameObject.layer);
                Debug.Log(hit.collider.gameObject.layer == 9);
            }

            if (hits.Length == 1 && hits[0].collider.gameObject.layer == 9)
                {
                    Debug.Log("hit obstacle!!!!!!!!!!!!!!!!!!");
                }
    }
    }

    public void DeleteMe()
    {
        switch (isLock)
        {
            case 0:
                SpawnManager.Instance.SendMessage("Despawn", gameObject);
                break;
            case 1:
                isLock = 0;
                lockSprite.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (pleaseDrop)
        {
            //spriteRenderer.color = Color.black;
            Drop();
        }
        if (colorEnum.Equals(colorText.rainbow))
        {
            
            if (gradientIncreasing)
            {
                t += Time.deltaTime / duration;
                if (t > 1f) gradientIncreasing = false;
            }
            else
            {
                t -= Time.deltaTime / duration;
                if (t < 0f) gradientIncreasing = true;
            }
            
            float value = Mathf.Lerp(0f, 1f, t);
            
            spriteRenderer.color = gradient.Evaluate(value);
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

        if (hits.Length == 1 && hits[0].collider.gameObject.layer == 7 && isLock < 1)
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