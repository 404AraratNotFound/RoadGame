using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour
{
    public AudioSource gameOverAudioSource;
    public string gameOverResourcesPath = "GameOver";

    public void PlayGameOverSound()
    {
        if (gameOverAudioSource == null)
            return;

        var clips = Resources.LoadAll<AudioClip>(gameOverResourcesPath);
        if (clips == null || clips.Length == 0)
            return;

        gameOverAudioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

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
            MusicManager.Instance.QueueMusicOnNextSceneLoad();
            MusicManager.Instance.QueueStartVoiceOnNextSceneLoad();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
