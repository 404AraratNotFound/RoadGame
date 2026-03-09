using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    public float destroyZ = -20f;

    void Update()
    {
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
