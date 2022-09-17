using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsRange : MonoBehaviour
{
    [SerializeField] List<Collider2D> myColliders = new List<Collider2D>();
    public void Explode()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Avocado"));
        List<Collider2D> result = new List<Collider2D>();
        List<Collider2D> results = new List<Collider2D>();



        foreach (Collider2D collider in myColliders)
        {
            Physics2D.OverlapCollider(collider, filter, result);
            results.AddRange(result);
        }

        foreach(Collider2D c in results)
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
