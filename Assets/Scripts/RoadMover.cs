using UnityEngine;

public class RoadMover : MonoBehaviour
{
    public bool canMove = false;
    public float speed = 10f;

    public float acceleration = 0.5f;

    void Update()
    {
        if (canMove)
        {
            speed += acceleration * Time.deltaTime;

            transform.Translate(Vector3.back * speed * Time.deltaTime);

            ScoreManager.Instance.AddScore(speed * Time.deltaTime);
        }
    }
}
