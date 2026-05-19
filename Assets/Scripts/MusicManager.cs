using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSource;
    public string musicResourcesPath = "Music";
    public float defaultVolume = 1f;
    public bool dontDestroyOnLoad = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        PlayRandom();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySavedVolume();
    }

    public void SetVolume(float volume)
    {
        if (audioSource == null)
            return;

        audioSource.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void PauseMusic()
    {
        if (audioSource == null)
            return;

        audioSource.Pause();
    }

    public void ResumeMusic()
    {
        if (audioSource == null)
            return;

        audioSource.UnPause();
    }

    public void StopMusic()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
    }

    public void PlayRandom()
    {
        if (audioSource == null)
            return;

        var clips = Resources.LoadAll<AudioClip>(musicResourcesPath);
        if (clips == null || clips.Length == 0)
            return;

        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.loop = true;
        ApplySavedVolume();
        audioSource.Play();
    }

    void ApplySavedVolume()
    {
        if (audioSource == null)
            return;

        audioSource.volume = Mathf.Clamp01(PlayerPrefs.GetFloat("MusicVolume", defaultVolume));
    }

    public void PlayRandomIfStopped()
    {
        if (audioSource == null)
            return;

        if (!audioSource.isPlaying)
        {
            PlayRandom();
        }
    }
}
