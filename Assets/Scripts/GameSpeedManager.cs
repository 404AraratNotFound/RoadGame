using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    public RoadMover roadMover;

    [Header("Speed")]
    public float speed = 10f;
    public float acceleration = 0.2f;

    [Header("Step increase (optional)")]
    public bool useStepIncrease;
    [Min(0.1f)]
    public float stepIntervalSeconds = 5f;
    public float stepIncrease = 1f;

    float _nextStepTime;

    void Update()
    {
        if (roadMover == null)
            roadMover = FindFirstObjectByType<RoadMover>();

        if (roadMover == null)
            return;

        // Пока игра не началась (отсчёт) — не разгоняем.
        if (!roadMover.canMove)
        {
            _nextStepTime = Time.time + stepIntervalSeconds;
            roadMover.speed = speed;
            return;
        }

        if (useStepIncrease)
        {
            if (Time.time >= _nextStepTime)
            {
                speed += stepIncrease;
                _nextStepTime = Time.time + stepIntervalSeconds;
            }
        }
        else
        {
            speed += acceleration * Time.deltaTime;
        }

        roadMover.speed = speed;
    }
}
