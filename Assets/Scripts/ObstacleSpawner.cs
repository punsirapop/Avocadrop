using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> obstacles = new List<GameObject>();

    private void OnEnable()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    void HandlePhaseChanged(Phase phase)
    {
        if(phase == Phase.Preparation)
        {
            foreach(Transform obstaclePos in transform)
            {
                int obsIndex = Random.Range(0, obstacles.Count);
                int obsAngle = Random.Range(0, 4);

                Instantiate(obstacles[obsIndex], obstaclePos.position,
                    Quaternion.AngleAxis(obsAngle * 90, Vector3.forward),
                    obstaclePos);
            }
        }

        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
        PhaseManager.Instance.PhaseChange(Phase.Spawn);
    }
}
