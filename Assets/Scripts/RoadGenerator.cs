using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public float segmentLength = 20f;

    private List<Transform> segments = new List<Transform>();

    void Start()
    {
        // Собираем все дочерние сегменты
        foreach (Transform child in transform)
        {
            segments.Add(child);
        }
    }

    void Update()
    {
        Transform firstSegment = segments[0];

        if (firstSegment.position.z < -segmentLength)
        {
            Recycle();
        }
    }

    void Recycle()
    {
        Transform firstSegment = segments[0];
        segments.RemoveAt(0);

        Transform lastSegment = segments[segments.Count - 1];

        float newZ = lastSegment.position.z + segmentLength;

        firstSegment.position = new Vector3(0, 0, newZ);

        // Respawn obstacles and holes on the segment when it gets recycled to the front.
        ObstacleSpawner spawner = firstSegment.GetComponentInChildren<ObstacleSpawner>();
        if (spawner != null)
        {
            spawner.RespawnObstacles();
        }

        HoleSpawner holeSpawner = firstSegment.GetComponentInChildren<HoleSpawner>();
        if (holeSpawner != null)
        {
            holeSpawner.RespawnHoles();
        }

        segments.Add(firstSegment);
    }
}
