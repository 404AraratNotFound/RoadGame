using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 32f;
    public float sensitivity = 0.5f;
    public float roadHalfWidth = 4f; // половина ширины дороги
    public bool inputBlocked;

    Coroutine _unblockCoroutine;

    void Update()
    {
        if (inputBlocked)
            return;

        float input = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            input = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            input = 1f;

        input *= sensitivity;

        // Двигаем ТОЛЬКО по мировому X
        Vector3 pos = transform.position;
        pos.x += input * moveSpeed * Time.deltaTime;

        // Ограничиваем движение границами дороги
        pos.x = Mathf.Clamp(pos.x, -roadHalfWidth, roadHalfWidth);

        transform.position = pos;
    }

    public void BlockInput(float durationSeconds)
    {
        inputBlocked = true;

        if (_unblockCoroutine != null)
        {
            StopCoroutine(_unblockCoroutine);
        }

        _unblockCoroutine = StartCoroutine(UnblockAfterDelay(durationSeconds));
    }

    System.Collections.IEnumerator UnblockAfterDelay(float durationSeconds)
    {
        if (durationSeconds > 0f)
            yield return new WaitForSecondsRealtime(durationSeconds);

        inputBlocked = false;
        _unblockCoroutine = null;
    }
}
