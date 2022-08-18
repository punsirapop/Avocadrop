using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avocado : MonoBehaviour
{
    public Color color;
    public bool isExplored;

    GameObject fallingPoint, phaseManager;
    Transform pointCollection;
    Phase phase;
    bool isDropped = true;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void Start()
    {
        phaseManager = GameObject.Find("PhaseManager");
        pointCollection = GameObject.Find("PointCollection").transform;

        fallingPoint = new GameObject("fallingPoint");
        fallingPoint.transform.position = transform.position;
        fallingPoint.transform.parent = pointCollection;

        color = spriteRenderer.color;
    }

    private void Update()
    {
        if (phase == Phase.Drop) Drop();
    }
    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, fallingPoint.transform.position, 1f);
    }

    void HandlePhaseChanged(Phase newPhase)
    {
        phase = newPhase;

        isDropped = !(phase == Phase.Drop);
    }

    void Drop()
    {
        if (Vector2.Distance(transform.position, fallingPoint.transform.position) < .5f)
        {
            int layermask = ~(LayerMask.GetMask("Avocado")) & ~(LayerMask.GetMask("Grid"));
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1f, -layermask);
            Debug.Log(hits.Length);
        
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
}