using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    [Tooltip("Wheel transforms to rotate")]
    public Transform[] wheels;

    [Tooltip("Degrees per second")]
    public float rotationSpeed = 720f;

    [Tooltip("Local axis to rotate around (usually X)")]
    public Vector3 localAxis = Vector3.right;

    public bool isSpinning;

    void Update()
    {
        if (!isSpinning || wheels == null || wheels.Length == 0)
            return;

        float angle = rotationSpeed * Time.deltaTime;
        for (int i = 0; i < wheels.Length; i++)
        {
            Transform w = wheels[i];
            if (w != null)
                w.Rotate(localAxis, angle, Space.Self);
        }
    }

    public void SetSpinning(bool spinning)
    {
        isSpinning = spinning;
    }
}
