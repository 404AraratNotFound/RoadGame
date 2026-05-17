using UnityEngine;

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

    public void SetVolume(float volume)
    {
        if (audioSource == null)
            return;

        audioSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
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
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
        audioSource.Play();
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
