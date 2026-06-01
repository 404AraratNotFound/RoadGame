using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI countdownText;
    public RoadMover roadMover;
    public WheelRotator wheelRotator;

    [Header("UI")]
    public GameObject gameOverUI;

    Coroutine _countdownCoroutine;

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        _countdownCoroutine = StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        roadMover.canMove = false;
        if (wheelRotator != null)
            wheelRotator.SetSpinning(false);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        roadMover.canMove = true;
        if (wheelRotator != null)
            wheelRotator.SetSpinning(true);
    }

    public void GameOver()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopMusic();
        }

        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        if (roadMover != null)
            roadMover.canMove = false;

        if (wheelRotator != null)
            wheelRotator.SetSpinning(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        var gameOverMenu = FindObjectOfType<GameOverMenuManager>();
        if (gameOverMenu != null)
        {
            gameOverMenu.PlayGameOverSound();
        }

        // Stop gameplay updates.
        Time.timeScale = 0f;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.SaveBestScore();
    }
}
