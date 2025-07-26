using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float initialYPosition = 10f;
    public float zoomSpeed = 10f;
    public float minYPosition = 5f;
    public float maxYPosition = 50f;
    public float lookSpeed = 2f;
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    private float currentYPosition;

    void Start()
    {
        // Initialize the camera's Y position
        currentYPosition = initialYPosition;
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
    }

    void Update()
    {
        if(!publicBoolForPauseMenuOpen.Check())
        {
            // Handle camera movement (WASD/Arrow keys) relative to the camera's orientation
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Get the forward and right directions relative to the camera's current rotation
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            // Keep the movement on the XZ plane (ignore Y component)
            forward.y = 0;
            right.y = 0;

            // Normalize to ensure consistent movement speed in all directions
            forward.Normalize();
            right.Normalize();

            // Calculate the direction to move based on input
            // Apply the movement to the camera's position
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Vector3 movement = (forward * verticalInput + right * horizontalInput) * moveSpeed * 1.5f * Time.deltaTime;
                transform.position += movement;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                Vector3 movement = (forward * verticalInput + right * horizontalInput) * moveSpeed * 2f * Time.deltaTime;
                transform.position += movement;
            }
            else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                Vector3 movement = (forward * verticalInput + right * horizontalInput) * moveSpeed * 4f * Time.deltaTime;
                transform.position += movement;
            }
            else
            {
                Vector3 movement = (forward * verticalInput + right * horizontalInput) * moveSpeed * Time.deltaTime;
                transform.position += movement;
            }
            // Keep the camera at the fixed Y position
            transform.position = new Vector3(transform.position.x, currentYPosition, transform.position.z);

            // Handle zooming by changing the height (Y position) with scroll wheel
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            currentYPosition -= scrollInput * zoomSpeed;
            currentYPosition = Mathf.Clamp(currentYPosition, minYPosition, maxYPosition);

            // Handle looking around with right mouse button (RMB)
            if (Input.GetMouseButton(1)) // RMB is pressed
            {
                float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
                float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

                // Rotate the camera based on mouse movement
                transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
            }
        }
    }
}
