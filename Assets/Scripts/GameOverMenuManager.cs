using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverMenuManager : MonoBehaviour
{
    public AudioSource gameOverAudioSource;
    public string gameOverResourcesPath = "GameOver";
    [Range(0f, 1f)]
    public float gameOverVideoChance = 0.25f;
    public string gameOverVideoResourcesPath = "GameOverVideo";
    public RawImage gameOverVideoRawImage;
    public Canvas gameOverVideoCanvas;
    public bool autoCreateVideoOverlay = true;

    VideoPlayer _videoPlayer;
    RenderTexture _renderTexture;
    GameObject _runtimeOverlayRoot;

    public void PlayGameOverSound()
    {
        if (TryPlayRandomGameOverVideo())
            return;

        PlayRandomGameOverAudio();
    }

    void PlayRandomGameOverAudio()
    {
        if (gameOverAudioSource == null)
            return;

        var clips = Resources.LoadAll<AudioClip>(gameOverResourcesPath);
        if (clips == null || clips.Length == 0)
            return;

        gameOverAudioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    bool TryPlayRandomGameOverVideo()
    {
        if (Random.value > gameOverVideoChance)
            return false;

        var clips = Resources.LoadAll<VideoClip>(gameOverVideoResourcesPath);
        if (clips == null || clips.Length == 0)
            return false;

        VideoClip clip = clips[Random.Range(0, clips.Length)];
        if (clip == null)
            return false;

        EnsureVideoOverlay();
        if (gameOverVideoRawImage == null || _videoPlayer == null)
            return false;

        CleanupVideoPlayback();

        _renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        _renderTexture.Create();

        gameOverVideoRawImage.texture = _renderTexture;
        gameOverVideoRawImage.gameObject.SetActive(true);
        gameOverVideoRawImage.rectTransform.anchorMin = Vector2.zero;
        gameOverVideoRawImage.rectTransform.anchorMax = Vector2.one;
        gameOverVideoRawImage.rectTransform.offsetMin = Vector2.zero;
        gameOverVideoRawImage.rectTransform.offsetMax = Vector2.zero;

        _videoPlayer.source = VideoSource.VideoClip;
        _videoPlayer.clip = clip;
        _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        _videoPlayer.targetTexture = _renderTexture;
        _videoPlayer.isLooping = false;
        _videoPlayer.playOnAwake = false;
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.SetTargetAudioSource(0, GetOrCreateVideoAudioSource());
        _videoPlayer.EnableAudioTrack(0, true);
        _videoPlayer.loopPointReached += OnVideoFinished;
        _videoPlayer.Prepare();
        _videoPlayer.prepareCompleted += OnVideoPrepared;

        if (gameOverVideoCanvas != null)
        {
            gameOverVideoCanvas.gameObject.SetActive(true);
            gameOverVideoCanvas.transform.SetAsLastSibling();
        }

        return true;
    }

    void EnsureVideoOverlay()
    {
        if (gameOverVideoRawImage != null && gameOverVideoCanvas != null)
            return;

        if (!autoCreateVideoOverlay)
            return;

        if (_runtimeOverlayRoot == null)
        {
            _runtimeOverlayRoot = new GameObject("GameOverVideoOverlay");
            _runtimeOverlayRoot.transform.SetParent(transform, false);

            if (gameOverVideoCanvas == null)
            {
                gameOverVideoCanvas = _runtimeOverlayRoot.AddComponent<Canvas>();
                gameOverVideoCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _runtimeOverlayRoot.AddComponent<CanvasScaler>();
                _runtimeOverlayRoot.AddComponent<GraphicRaycaster>();
            }

            if (gameOverVideoRawImage == null)
            {
                GameObject rawImageObject = new GameObject("VideoRawImage");
                rawImageObject.transform.SetParent(gameOverVideoCanvas.transform, false);
                gameOverVideoRawImage = rawImageObject.AddComponent<RawImage>();
            }

            _videoPlayer = _runtimeOverlayRoot.AddComponent<VideoPlayer>();
            _videoPlayer.playOnAwake = false;
        }
    }

    AudioSource GetOrCreateVideoAudioSource()
    {
        AudioSource source = _runtimeOverlayRoot != null ? _runtimeOverlayRoot.GetComponent<AudioSource>() : null;
        if (source == null && _runtimeOverlayRoot != null)
        {
            source = _runtimeOverlayRoot.AddComponent<AudioSource>();
        }

        return source;
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        source.prepareCompleted -= OnVideoPrepared;
        source.Play();
    }

    void OnVideoFinished(VideoPlayer source)
    {
        CleanupVideoPlayback();
    }

    void CleanupVideoPlayback()
    {
        if (_videoPlayer != null)
        {
            _videoPlayer.prepareCompleted -= OnVideoPrepared;
            _videoPlayer.loopPointReached -= OnVideoFinished;
            _videoPlayer.Stop();
            _videoPlayer.clip = null;
        }

        if (gameOverVideoRawImage != null)
        {
            gameOverVideoRawImage.texture = null;
            gameOverVideoRawImage.gameObject.SetActive(false);
        }

        if (_renderTexture != null)
        {
            _renderTexture.Release();
            Destroy(_renderTexture);
            _renderTexture = null;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        CleanupVideoPlayback();
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        CleanupVideoPlayback();
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.QueueMusicOnNextSceneLoad();
            MusicManager.Instance.QueueStartVoiceOnNextSceneLoad();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
