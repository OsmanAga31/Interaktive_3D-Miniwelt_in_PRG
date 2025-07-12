using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Controls player movement, jumping, and rotation based on input and camera orientation.
/// Written in collaboration with teacher Bohdan Chumak and later edited by me to suit specific needs.
/// </summary>
public class PlayerController2 : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Transform cameraTransform;
    [SerializeField] private float actualMovementSpeed;
    [SerializeField] private float jumpStrength;
    private bool isGrounded;

    [Header("Player Rotation")]
    [SerializeField] private Transform cinemachineCameraTarget;

    /// <summary>
    /// Initializes input actions, camera reference, and Rigidbody.
    /// </summary>
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        playerInput.actions["Jump"].performed += OnJump;
        playerInput.actions["Jump"].canceled += OnJump;

        cameraTransform = Camera.main.transform;

        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Handles movement input from the player.
    /// </summary>
    /// <param name="ctx">Input callback context containing movement vector.</param>
    public void OnMove(CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles jump input from the player. Only allows jumping if grounded.
    /// </summary>
    /// <param name="ctx">Input callback context for jump action.</param>
    public void OnJump(CallbackContext ctx)
    {
        if (!ctx.started && !isGrounded) return;
        rb.AddForce(Vector3.up * jumpStrength);
    }

    /// <summary>
    /// Handles physics-based movement and checks if the player is grounded.
    /// </summary>
    void FixedUpdate()
    {
        // Raycast down to check if the player is grounded
        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(
            transform.position.x,
            transform.position.y - (transform.gameObject.GetComponent<CapsuleCollider>().height / 2) + 0.05f,
            transform.position.z
        );
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, 0.15f);
        Debug.DrawRay(rayOrigin, Vector3.down * 0.15f, Color.red, 0.1f);

        // Calculate movement direction relative to camera orientation
        var movementDirection = cameraTransform.right * movementInput.x + cameraTransform.forward * movementInput.y;
        movementDirection = Vector3.ProjectOnPlane(movementDirection, Vector3.up).normalized;

        // Move the player using Rigidbody for smooth physics interaction
        rb.MovePosition(rb.position + movementDirection * Time.fixedDeltaTime * actualMovementSpeed);
    }

    /// <summary>
    /// Rotates the player to face the direction of the camera's Y (yaw) axis.
    /// Called in LateUpdate to ensure camera has updated.
    /// </summary>
    private void PlayerRotation()
    {
        if (cinemachineCameraTarget == null) return;

        Vector3 targetEuler = cinemachineCameraTarget.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, targetEuler.y, 0f);
    }

    /// <summary>
    /// Ensures player rotation is updated after camera movement.
    /// </summary>
    private void LateUpdate()
    {
        PlayerRotation();
    }
}
