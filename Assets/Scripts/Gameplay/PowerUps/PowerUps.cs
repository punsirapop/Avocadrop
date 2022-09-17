using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUps : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public GameObject Range;

    Collider2D grid;
    Vector3 _position, cursorPos;
    bool isSelected;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        Range = null;
    }

    private void Update()
    {
        if(Range.name == "Color")
        {
            grid = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Avocado"));
        }
        else
        {
            grid = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Grid"));
        }
        
        if(Range != null && isSelected)
        {
            if (grid != null)
            {
                Range.transform.position = grid.transform.position;
                Range.SendMessage("ChangePos", SendMessageOptions.DontRequireReceiver);
                Range.SetActive(true);
            }
            else
            {
                Range.SetActive(false);
            }
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSelected = true;
        // Range.SetActive(isSelected);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isSelected = false;
        if (PhaseManager.Instance.phase != Phase.PlayerAction)
        {
            transform.position = _position;
            Range.SetActive(false);
            return;
        }

        if (Range.activeSelf)
        {
            // PowerUpsManager.Instance.usePowerUp(gameObject);
            Range.SendMessage("Explode");
            transform.position = _position;
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = _position;
        }
        // Range.SetActive(isSelected);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (PhaseManager.Instance.phase == Phase.PlayerAction && isSelected)
        {
            cursorPos = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.position = new Vector2(cursorPos.x, cursorPos.y);
        }
    }

    public void SetRange(GameObject range)
    {
        Range = range;
        _position = transform.position;
        /*
        Range = Instantiate(Range, transform.position, Quaternion.identity);
        Range.SetActive(false);
        */
    }
}
