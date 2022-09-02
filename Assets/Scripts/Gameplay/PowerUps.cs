using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUps : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public GameObject Range;

    GameObject _Range;
    Vector3 _position, cursorPos;
    bool isSelected;

    private void Start()
    {
        _position = transform.position;
        _Range = Instantiate(Range, transform.position, Quaternion.identity);
        _Range.SetActive(false);
    }

    private void Update()
    {
        Collider2D grid = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Grid"));

        if(grid != null && isSelected)
        {
            _Range.transform.position = grid.transform.position;
            _Range.SetActive(true);
        }
        else
        {
            _Range.SetActive(false);
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

        if (_Range.activeSelf)
        {
            PowerUpsManager.Instance.usePowerUp(gameObject);
            _Range.SendMessage("Explode");
            transform.position = _position;
            Destroy(gameObject);
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
}
