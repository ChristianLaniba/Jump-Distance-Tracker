using UnityEngine;
using TMPro; // Added for UI

public class PlayerMovement : MonoBehaviour
{
  public float speed = 5f;
  public float jumpHeight = 2f;
  public float gravity = -9.81f;

  public Transform groundCheck;
  public float groundDistance = 0.4f;
  public LayerMask groundMask;

  [Header("Camera & UI Setup")]
  public Transform cameraTransform; // Drag Main Camera here
  public TextMeshProUGUI distanceText; // Drag Text (TMP) here

  private CharacterController controller;
  private Vector3 velocity;
  private bool isGrounded;
  private Vector3 lastPos;
  
  private Vector3 startPosition;

  void Start()
  {
    controller = GetComponent<CharacterController>();
    lastPos = transform.position;
    startPosition = transform.position; // Store start pos for distance
  }

  void Update()
  {
    // Ground check
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    // Reset falling velocity when grounded
    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    // Movement
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    // NEW: Get camera's forward direction (flattened so we don't move up/down)
    Vector3 camForward = cameraTransform.forward;
    camForward.y = 0f;
    camForward.Normalize();
    
    Vector3 camRight = cameraTransform.right;
    camRight.y = 0f;
    camRight.Normalize();

    // Move relative to the camera
    Vector3 move = (camForward * z) + (camRight * x);
    
    // NEW: Rotate player to face the direction they are moving
    if (move.magnitude > 0.1f)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 12f * Time.deltaTime);
    }

    controller.Move(move * speed * Time.deltaTime);

    // Jump
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    // Gravity
    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);

    // Position tracking
    Vector3 currentPos = transform.position;
    if (currentPos != lastPos)
    {
      // Debug.Log($"Player moved from {lastPos} to {currentPos}");
      lastPos = currentPos;
    }

    // NEW: Distance Tracking UI
    if (distanceText != null)
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        distanceText.text = "Distance: " + distance.ToString("F2") + " m";
    }
  }
}