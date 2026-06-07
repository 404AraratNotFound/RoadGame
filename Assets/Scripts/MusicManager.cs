using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSource;
    public AudioSource voiceAudioSource;
    public AudioSource fillVoiceAudioSource;
    public string musicResourcesPath = "Music";
    public string startVoiceResourcesPath = "Voice/Start";
    public string fillVoiceResourcesPath = "Voice/Fill";
    public float defaultVolume = 1f;
    public float voiceVolume = 1f;
    public bool playStartVoiceOnStart = true;
    public bool dontDestroyOnLoad = true;
    bool playStartVoiceOnNextSceneLoad;
    bool playMusicOnNextSceneLoad;

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

        if (playStartVoiceOnStart)
        {
            PlayRandomStartVoice();
        }
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

        if (playMusicOnNextSceneLoad)
        {
            playMusicOnNextSceneLoad = false;
            PlayRandom();
        }

        if (playStartVoiceOnNextSceneLoad)
        {
            playStartVoiceOnNextSceneLoad = false;
            PlayRandomStartVoice();
        }
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

    public void PlayRandomStartVoice()
    {
        PlayRandomVoice(startVoiceResourcesPath);
    }

    public void QueueStartVoiceOnNextSceneLoad()
    {
        playStartVoiceOnNextSceneLoad = true;
    }

    public void QueueMusicOnNextSceneLoad()
    {
        playMusicOnNextSceneLoad = true;
    }

    public void PlayRandomFillVoice()
    {
        PlayRandomVoice(fillVoiceResourcesPath, fillVoiceAudioSource);
    }

    void PlayRandomVoice(string resourcesPath, AudioSource preferredSource = null)
    {
        AudioSource source = preferredSource != null ? preferredSource : voiceAudioSource != null ? voiceAudioSource : audioSource;
        if (source == null)
            return;

        var clips = Resources.LoadAll<AudioClip>(resourcesPath);
        if (clips == null || clips.Length == 0)
            return;

        source.PlayOneShot(clips[Random.Range(0, clips.Length)], Mathf.Clamp01(voiceVolume));
    }
}
