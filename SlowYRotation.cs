using UnityEngine;

public class SlowYRotation : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 20f;

    void Update()
    {
        // Rotate around the Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}

