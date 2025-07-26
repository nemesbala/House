using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    Vector3 lookPos;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            lookPos = mainCamera.transform.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
            transform.Rotate(0, 180f, 0); // Flip to face camera properly
        }
    }
}