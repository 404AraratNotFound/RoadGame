using UnityEngine;

public class RoadMover : MonoBehaviour
{
    public bool canMove = false;
    public float speed = 10f;

    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
