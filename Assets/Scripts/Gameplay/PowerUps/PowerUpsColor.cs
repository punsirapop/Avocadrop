using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

public class PowerUpsColor : MonoBehaviour
{
    [SerializeField] List<Collider2D> myColliders = new List<Collider2D>();
    [SerializeField] GameObject Single;
    [SerializeField] Transform SingleCollection;

    int capacity = 10, max = 20;
    ObjectPool<GameObject> pool;

    ContactFilter2D filter = new ContactFilter2D();
    List<Collider2D> result = new List<Collider2D>();
    List<Collider2D> results = new List<Collider2D>();
    List<Collider2D> deleteThese = new List<Collider2D>();

    private void OnEnable()
    {
        if(pool == null)
        {
            Debug.Log("GEN NEW POOL");

            pool = new ObjectPool<GameObject>(
            () => { return Instantiate(Single, SingleCollection); },
            s => {
                s.gameObject.SetActive(true);
            },
            s => {
                s.gameObject.SetActive(false);
            },
            s => { Destroy(s.gameObject); },
            false, capacity, max);
        }
    }

    private void OnDisable()
    {
        foreach(Transform t in SingleCollection)
        {
            pool.Release(t.gameObject);
        }
    }

    public void ChangePos()
    {
        foreach (Transform t in SingleCollection)
        {
            pool.Release(t.gameObject);
        }

        results.Clear();
        deleteThese.Clear();

        filter.SetLayerMask(LayerMask.GetMask("Avocado"));
        foreach (Collider2D collider in myColliders)
        {
            Physics2D.OverlapCollider(collider, filter, result);
            results.AddRange(result);
        }

        Color myColor = results.Find(x => (x.transform.position == transform.position)).GetComponent<Avocado>().color;
        foreach (Collider2D c in results)
        {
            if (c.GetComponent<Avocado>().color == myColor)
            {
                deleteThese.Add(c);
                GameObject single = pool.Get();
                single.transform.position = c.transform.position;
            }
        }
    }

    public void Explode()
    {
        foreach (Collider2D c in deleteThese)
        {
            c.SendMessage("DeleteMe");
        }

        PhaseManager.Instance.PhaseChange(Phase.Drop);
        gameObject.SetActive(false);
    }
}
