using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

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
        SceneManager.LoadScene(1);
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
