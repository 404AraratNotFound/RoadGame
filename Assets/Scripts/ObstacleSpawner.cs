using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public Transform[] spawnPoints;

    [Header("Rules")]
    [Range(0f, 1f)]
    public float spawnChancePerPoint = 0.5f;

    [Tooltip("If enabled, at least 1 lane will always remain empty.")]
    public bool keepAtLeastOneLaneFree = true;

    [Tooltip("0 = no limit. If keepAtLeastOneLaneFree is enabled, the limit will be applied automatically.")]
    [Min(0)]
    public int maxObstaclesPerSegment = 0;

    void Start()
    {
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        if (obstacles == null || obstacles.Length == 0 || spawnPoints == null || spawnPoints.Length == 0)
            return;

        int maxAllowed = maxObstaclesPerSegment;
        if (keepAtLeastOneLaneFree)
            maxAllowed = maxAllowed == 0 ? spawnPoints.Length - 1 : Mathf.Min(maxAllowed, spawnPoints.Length - 1);

        // Collect candidate lanes by chance.
        int[] candidates = new int[spawnPoints.Length];
        int candidateCount = 0;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null)
                continue;

            if (Random.value <= spawnChancePerPoint)
                candidates[candidateCount++] = i;
        }

        // If we ended up blocking all lanes, drop one at random.
        if (keepAtLeastOneLaneFree && candidateCount >= spawnPoints.Length)
        {
            int removeIndex = Random.Range(0, candidateCount);
            candidates[removeIndex] = candidates[candidateCount - 1];
            candidateCount--;
        }

        // Enforce max obstacles per segment.
        if (maxAllowed > 0 && candidateCount > maxAllowed)
        {
            // Partial shuffle to pick a random subset of size maxAllowed.
            for (int i = 0; i < maxAllowed; i++)
            {
                int j = Random.Range(i, candidateCount);
                (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
            }

            candidateCount = maxAllowed;
        }

        for (int n = 0; n < candidateCount; n++)
        {
            Transform point = spawnPoints[candidates[n]];
            if (point == null)
                continue;

            int randomObstacle = Random.Range(0, obstacles.Length);
            if (obstacles[randomObstacle] == null)
                continue;

            Instantiate(obstacles[randomObstacle], point.position, Quaternion.identity, transform);
        }
    }
}
