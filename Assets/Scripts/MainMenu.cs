using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string settingsSceneName = "Settings";

    public void NewGame()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayRandom();
        }
        SceneManager.LoadScene(2);
    }
    public void Settings()
    {
        if (string.IsNullOrEmpty(settingsSceneName))
            return;

        var settingsScene = SceneManager.GetSceneByName(settingsSceneName);
        if (settingsScene.isLoaded)
            return;

        SceneManager.LoadSceneAsync(settingsSceneName, LoadSceneMode.Additive);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
