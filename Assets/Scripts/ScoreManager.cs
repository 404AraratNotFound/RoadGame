using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI recordText;

    [Header("Scoring")]
    [Tooltip("How many points are earned per 1 world unit of distance")]
    public float pointsPerUnitDistance = 1f;

    [Tooltip("Optional. If not set, will be found at runtime.")]
    public RoadMover roadMover;

    float _score;
    int _bestScore;

    const string BestScoreKey = "BestScore";
    const string LegacyRecordKey = "Record";

    void Awake()
    {
        Instance = this;

        _bestScore = PlayerPrefs.GetInt(BestScoreKey, PlayerPrefs.GetInt(LegacyRecordKey, 0));
        UpdateUI();
    }

    void Update()
    {
        if (roadMover == null)
            roadMover = FindFirstObjectByType<RoadMover>();

        if (roadMover == null || !roadMover.canMove)
            return;

        float distance = roadMover.speed * Time.deltaTime;
        AddDistance(distance);
    }

    public void AddDistance(float distance)
    {
        if (distance <= 0f)
            return;

        AddScore(distance * pointsPerUnitDistance);
    }

    public void AddPoints(int points)
    {
        if (points <= 0)
            return;

        AddScore(points);
    }

    public void AddScore(float amount)
    {
        if (amount <= 0f)
            return;

        _score += amount;

        int current = Mathf.FloorToInt(_score);
        if (current > _bestScore)
        {
            _bestScore = current;
            SaveBestScore();
        }

        UpdateUI();
    }


    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Счёт: " + Mathf.FloorToInt(_score);

        if (recordText != null)
            recordText.text = "Рекорд: " + _bestScore;
    }

    public void SaveBestScore()
    {
        PlayerPrefs.SetInt(BestScoreKey, _bestScore);
        PlayerPrefs.SetInt(LegacyRecordKey, _bestScore);
        PlayerPrefs.Save();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveBestScore();
    }

    void OnApplicationQuit()
    {
        SaveBestScore();
    }
}
