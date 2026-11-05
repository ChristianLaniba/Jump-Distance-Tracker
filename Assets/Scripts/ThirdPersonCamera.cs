using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;              // Drag Player here
    public float distance = 5f;           // How far behind the player
    public float height = 2f;             // How high above the player
    public float mouseSensitivity = 3f;
    public float smoothSpeed = 10f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float yaw = 0f;
    private float pitch = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // Hide + lock cursor
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Mouse input
        yaw   += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch  = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 2. Calculate camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPos = target.position + Vector3.up * height + offset;

        // 3. Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        
        // 4. Look at the player
        transform.LookAt(target.position + Vector3.up * height);
    }
}