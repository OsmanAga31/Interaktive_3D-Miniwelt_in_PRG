using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController2 : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Transform cameraTransform;
    [SerializeField] private float actualMovementSpeed;
    [SerializeField] private float jumpStrength;
    private bool isGrounded;

    // Start is called once before the first exexution of Update after the MonoBehavior is enabled.
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        playerInput.actions["Jump"].performed += OnJump;

        cameraTransform = Camera.main.transform;

        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext ctx)
    {
        if (!isGrounded) return; // Only allow jumping if grounded
        rb.AddForce(Vector3.up * jumpStrength);
        Debug.Log("Jumped!");
    }

    // Update is called once per frame
    void Update()
    {
        var movementDirection = cameraTransform.right * movementInput.x + cameraTransform.forward * movementInput.y;
        movementDirection = Vector3.ProjectOnPlane(movementDirection, Vector3.up).normalized; // Project onto the horizontal plane
        transform.Translate(movementDirection * Time.deltaTime * actualMovementSpeed);
        //rb.MovePosition(movementDirection * Time.deltaTime * actualMovementSpeed);
        //transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime * actualMovementSpeed);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
