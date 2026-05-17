using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayRandom();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
