using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class FPSController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera fpsCamera;
    [SerializeField] private PlayerInput playerInput;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float airControl = 0.3f;
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxLookAngle = 89f;

    private Rigidbody rb;
    private CapsuleCollider col;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpRequested;
    private bool readyToJump = true;
    private bool grounded;
    private bool isCrouching;
    private bool isSprinting;
    private float xRotation;
    private float originalHeight;
    private Vector3 cameraOriginalPosition;
    private float footstepTimer;
    private float currentSpeed;

    // Public properties for UI
    public Vector2 MoveInput => moveInput;
    public Vector2 LookInput => lookInput;
    public bool IsJumpRequested => jumpRequested;
    public bool IsCrouching => isCrouching;
    public bool IsGrounded => grounded;
    public bool IsSprinting => isSprinting;
    public float CurrentSpeed => currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        originalHeight = col.height;

        // Store original camera position
        if (fpsCamera != null)
        {
            cameraOriginalPosition = fpsCamera.transform.localPosition;
        }

        LockCursor();
        SetupRigidbody();
    }

    private void SetupRigidbody()
    {
        rb.mass = 75f;
        rb.linearDamping = groundDrag;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GroundCheck();
        HandleCrouch();
        UpdateCameraPosition();
        HandleHeadBob();
        currentSpeed = rb.linearVelocity.magnitude;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleLookRotation();
        HandleDrag();
        HandleGravity();
        HandleJump();
    }

    private void GroundCheck()
    {
        float sphereRadius = col.radius * 0.9f;
        float sphereY = col.bounds.min.y + sphereRadius;
        Vector3 spherePos = new Vector3(transform.position.x, sphereY, transform.position.z);

        grounded = Physics.CheckSphere(spherePos, sphereRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    private void HandleLookRotation()
    {
        if (lookInput.sqrMagnitude < 0.01) return;

        // Horizontal rotation (player body)
        float mouseX = lookInput.x * mouseSensitivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Vertical rotation (Cinemachine camera)
        float mouseY = lookInput.y * mouseSensitivity * Time.fixedDeltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        // Apply rotation to Cinemachine camera
        if (fpsCamera != null)
        {
            fpsCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        if (moveInput.sqrMagnitude < 0.01) return;

        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        float controlMultiplier = grounded ? 1f : airControl;
        float speedMultiplier = isCrouching ? crouchSpeedMultiplier : 1f;
        speedMultiplier *= isSprinting ? sprintMultiplier : 1f;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = moveDirection * moveSpeed * speedMultiplier;
        targetVelocity.y = currentVelocity.y; // Preserve vertical velocity

        Vector3 velocityChange = (targetVelocity - currentVelocity) * acceleration * controlMultiplier;
        velocityChange = Vector3.ClampMagnitude(velocityChange, moveSpeed * sprintMultiplier);
        velocityChange.y = 0; // Only affect horizontal movement

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void HandleDrag()
    {
        rb.linearDamping = grounded ? groundDrag : airDrag;
    }

    private void HandleGravity()
    {
        if (!grounded && rb.linearVelocity.y < 0)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1) * rb.mass);
        }
    }

    private void HandleCrouch()
    {
        if (isCrouching)
        {
            col.height = Mathf.Lerp(col.height, crouchHeight, 10f * Time.deltaTime);
        }
        else
        {
            col.height = Mathf.Lerp(col.height, originalHeight, 10f * Time.deltaTime);
        }
    }

    private void UpdateCameraPosition()
    {
        if (fpsCamera == null) return;

        // Calculate camera position based on player height
        float targetHeight = col.height - 0.1f;
        Vector3 targetPosition = cameraOriginalPosition;
        targetPosition.y = targetHeight;

        // Smoothly adjust camera position
        fpsCamera.transform.localPosition = Vector3.Lerp(
            fpsCamera.transform.localPosition,
            targetPosition,
            10f * Time.deltaTime
        );
    }

    private void HandleHeadBob()
    {
        if (!grounded || moveInput.sqrMagnitude < 0.01) return;

        // Simple head bob effect
        footstepTimer += Time.deltaTime * (isSprinting ? 1.5f : 1f);
        float bobAmount = Mathf.Sin(footstepTimer) * 0.05f * (isSprinting ? 1.2f : 1f);

        if (fpsCamera != null)
        {
            Vector3 pos = fpsCamera.transform.localPosition;
            pos.y = cameraOriginalPosition.y + bobAmount;
            fpsCamera.transform.localPosition = pos;
        }
    }

    private void HandleJump()
    {
        if (jumpRequested && readyToJump && grounded && !isCrouching)
        {
            readyToJump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
            jumpRequested = false;
        }
    }

    private void ResetJump() => readyToJump = true;

    // Input System Event Handlers (assign in inspector)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpRequested = true;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ?
                CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!col) return;

        Gizmos.color = grounded ? Color.green : Color.red;
        float sphereRadius = col.radius * 0.9f;
        float sphereY = col.bounds.min.y + sphereRadius;
        Vector3 spherePos = new Vector3(transform.position.x, sphereY, transform.position.z);
        Gizmos.DrawWireSphere(spherePos, sphereRadius);
    }
}