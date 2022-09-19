using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUpsColor : MonoBehaviour
{
    [SerializeField] List<Collider2D> myColliders = new List<Collider2D>();
    [SerializeField] GameObject Single;

    Transform SingleCollection;
    ContactFilter2D filter = new ContactFilter2D();
    List<Collider2D> result = new List<Collider2D>();
    List<Collider2D> results = new List<Collider2D>();
    List<Collider2D> deleteThese = new List<Collider2D>();

    private void OnEnable()
    {
        SingleCollection = GameObject.Find("SingleCollection").transform;
    }

    private void OnDisable()
    {
        foreach(Transform t in SingleCollection)
        {
            PowerUpsSinglePool.singlePool.Release(t.gameObject);
        }
    }

    public void ChangePos()
    {
        foreach (Transform t in SingleCollection)
        {
            PowerUpsSinglePool.singlePool.Release(t.gameObject);
        }

        results.Clear();
        deleteThese.Clear();

        filter.SetLayerMask(LayerMask.GetMask("Avocado"));
        foreach (Collider2D collider in myColliders)
        {
            Physics2D.OverlapCollider(collider, filter, result);
            results.AddRange(result);
        }

        Avocado.colorText myColor = results.Find
            (x => (x.transform.position == transform.position)).GetComponent<Avocado>().colorEnum;
        foreach (Collider2D c in results)
        {
            if (c.GetComponent<Avocado>().colorEnum == myColor)
            {
                deleteThese.Add(c);
                GameObject single = PowerUpsSinglePool.singlePool.Get();
                single.transform.position = c.transform.position;
            }
        }
    }

    public void Explode()
    {
        foreach (Collider2D c in deleteThese)
        {
            if (!BoardState.isRerolling)
            {
                Debug.Log("points from powerup");
                BoardState.currentScore += 10;
            }
            c.SendMessage("DeleteMe");
        }
        if (BoardState.isRerolling)
        {
            BoardState.isRerolling = false;
        }
        PhaseManager.Instance.PhaseChange(Phase.Drop);
        gameObject.SetActive(false);
    }
}
