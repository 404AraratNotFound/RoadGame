using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public GameObject pauseMenu;
    public GameObject scoreUi;
    public string settingsSceneName = "Settings";
    public int settingsSortingOrder = 1000;

    bool isPaused = false;

    void Awake()
    {
        Instance = this;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        if (scoreUi != null)
        {
            scoreUi.SetActive(false);
        }
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PauseMusic();
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        if (scoreUi != null)
        {
            scoreUi.SetActive(true);
        }
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.ResumeMusic();
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ShowPauseMenu()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }

        if (scoreUi != null)
        {
            scoreUi.SetActive(false);
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopMusic();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        if (string.IsNullOrEmpty(settingsSceneName))
            return;

        var settingsScene = SceneManager.GetSceneByName(settingsSceneName);
        if (settingsScene.isLoaded)
            return;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        StartCoroutine(LoadSettingsScene());
    }

    IEnumerator LoadSettingsScene()
    {
        var loadOperation = SceneManager.LoadSceneAsync(settingsSceneName, LoadSceneMode.Additive);
        if (loadOperation == null)
            yield break;

        while (!loadOperation.isDone)
            yield return null;

        var settingsScene = SceneManager.GetSceneByName(settingsSceneName);
        if (!settingsScene.IsValid())
            yield break;

        SceneManager.SetActiveScene(settingsScene);

        var rootObjects = settingsScene.GetRootGameObjects();
        foreach (var root in rootObjects)
        {
            var canvases = root.GetComponentsInChildren<Canvas>(true);
            foreach (var canvas in canvases)
            {
                canvas.overrideSorting = true;
                canvas.sortingOrder = settingsSortingOrder;
            }
        }
    }
}

