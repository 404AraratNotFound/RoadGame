using UnityEngine;

public class Hole : MonoBehaviour
{
    public int points = 500;
    [Range(0.1f, 1f)]
    public float slowMultiplier = 0.4f;
    public float slowDurationSeconds = 1f;
    public float inputBlockDurationSeconds = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayRandomFillVoice();
        }

        var speedManager = FindFirstObjectByType<GameSpeedManager>();
        if (speedManager != null)
        {
            speedManager.ApplyTemporarySlow(slowMultiplier, slowDurationSeconds);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(points);
        }

        var playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.BlockInput(inputBlockDurationSeconds);
        }

        float destroyDelay = Mathf.Max(0f, slowDurationSeconds * 0.5f);
        Destroy(gameObject, destroyDelay);
    }

}
