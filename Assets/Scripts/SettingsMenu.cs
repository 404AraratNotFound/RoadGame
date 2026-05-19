using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public string settingsSceneName = "Settings";
    public Slider volumeSlider;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
            volumeSlider.onValueChanged.AddListener(SetMusicVolume);
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SetMusicVolume(volumeSlider.value);
        }
    }

    public void Back()
    {
        if (string.IsNullOrEmpty(settingsSceneName))
        {
            return;
        }

        var settingsScene = SceneManager.GetSceneByName(settingsSceneName);
        if (settingsScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(settingsSceneName);
        }

        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.ShowPauseMenu();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(volume);
        }

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
}
