using UnityEngine;

public class HoleSpawner : MonoBehaviour
{
    public GameObject[] holes;
    public Transform[] spawnPoints;

    [Header("Rules")]
    [Range(0f, 1f)]
    public float spawnChancePerPoint = 0.5f;

    [Header("Runtime")]
    [Tooltip("Parent for spawned holes. If not set, it will be created automatically.")]
    public Transform spawnedRoot;

    void Start()
    {
        EnsureSpawnedRoot();
        RespawnHoles();
    }

    void EnsureSpawnedRoot()
    {
        if (spawnedRoot != null)
            return;

        Transform existing = transform.Find("SpawnedHoles");
        if (existing != null)
        {
            spawnedRoot = existing;
            return;
        }

        GameObject go = new GameObject("SpawnedHoles");
        spawnedRoot = go.transform;
        spawnedRoot.SetParent(transform, false);
    }

    public void RespawnHoles()
    {
        EnsureSpawnedRoot();

        if (spawnedRoot != null)
        {
            for (int i = spawnedRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(spawnedRoot.GetChild(i).gameObject);
            }
        }

        SpawnHoles();
    }

    void SpawnHoles()
    {
        if (holes == null || holes.Length == 0 || spawnPoints == null || spawnPoints.Length == 0)
            return;

        EnsureSpawnedRoot();

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        if (Random.value > spawnChancePerPoint)
            return;

        Transform point = spawnPoints[spawnIndex];
        if (point == null)
            return;

        int randomHole = Random.Range(0, holes.Length);
        if (holes[randomHole] == null)
            return;

        Instantiate(holes[randomHole], point.position, holes[randomHole].transform.rotation, spawnedRoot != null ? spawnedRoot : transform);
    }
}
