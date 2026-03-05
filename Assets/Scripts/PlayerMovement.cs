using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 32f;
    public float roadHalfWidth = 4f; // половина ширины дороги

    void Update()
    {
        float input = 0f;

#if UNITY_EDITOR
        // В редакторе используем клавиатуру
        input = Input.GetAxis("Horizontal");
#else
        // На телефоне используем свайп
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            input = touch.deltaPosition.x * 0.01f;
        }
#endif

        // Двигаем ТОЛЬКО по мировому X
        Vector3 pos = transform.position;
        pos.x += input * moveSpeed * Time.deltaTime;

        // Ограничиваем движение границами дороги
        pos.x = Mathf.Clamp(pos.x, -roadHalfWidth, roadHalfWidth);

        transform.position = pos;
    }
}
