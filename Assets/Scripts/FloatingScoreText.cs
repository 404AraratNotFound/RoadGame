using TMPro;
using UnityEngine;

public class FloatingScoreText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float moveUpDistance = 60f;
    public float duration = 0.8f;

    CanvasGroup _canvasGroup;
    Vector3 _startPos;
    float _time;

    void Awake()
    {
        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();

        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();

        _startPos = transform.localPosition;
    }

    void Update()
    {
        _time += Time.unscaledDeltaTime;
        float t = duration > 0f ? Mathf.Clamp01(_time / duration) : 1f;

        transform.localPosition = _startPos + Vector3.up * (moveUpDistance * t);
        _canvasGroup.alpha = 1f - t;

        if (t >= 1f)
            Destroy(gameObject);
    }

    public void SetText(string value)
    {
        if (text != null)
            text.text = value;
    }
}
