using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    //moving player in the direction the camera is looking at
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;

    [SerializeField] CinemachineCamera virtualCamera; // Reference to the Cinemachine virtual camera

    [SerializeField] private Camera cam; // Reference to the main camera
    [SerializeField] private Transform playerModel; // Reference to the player model
    public float rotationSpeed = 5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
   
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Reset grounded state after jumping
        }
    }

    public void FixedUpdate()
    {
        // Calculate movement direction based on camera orientation
        Vector3 forward = cam.transform.forward;
        forward.y = 0; // Ignore vertical component
        forward.Normalize(); // Normalize to ensure consistent speed
        Vector3 right = cam.transform.right;
        right.y = 0; // Ignore vertical component
        right.Normalize(); // Normalize to ensure consistent speed
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        // Move the player
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        // Rotate the player model to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        Vector3 cameraForward = virtualCamera.transform.forward;
        cameraForward.y = 0f;

        if (cameraForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }


}
