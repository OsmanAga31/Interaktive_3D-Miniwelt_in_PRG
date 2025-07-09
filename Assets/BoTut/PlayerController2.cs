using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
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

    [Header("Player Rotation")]
    [SerializeField] private Transform cinemachineCameraTarget;



    // Start is called once before the first exexution of Update after the MonoBehavior is enabled.
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

    public void OnMove(CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext ctx)
    {
        if (!ctx.started && !isGrounded) return; // Only allow jumping if grounded
        rb.AddForce(Vector3.up * jumpStrength);
        //Debug.Log("Jumped!");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // raycast down to check if the player is grounded
        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y - (transform.gameObject.GetComponent<CapsuleCollider>().height / 2) + 0.05f, transform.position.z); // Adjust the origin to the bottom of the capsule collider
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, 0.15f); // Adjust the distance as needed
        Debug.DrawRay(rayOrigin, Vector3.down*0.15f, Color.red, 0.1f); // Visualize the raycast in the editor
        //Debug.Log("Is Grounded: " + isGrounded);


        var movementDirection = cameraTransform.right * movementInput.x + cameraTransform.forward * movementInput.y;
        movementDirection = Vector3.ProjectOnPlane(movementDirection, Vector3.up).normalized; // Project onto the horizontal plane
        //transform.Translate(movementDirection * Time.deltaTime * actualMovementSpeed);
        rb.MovePosition(rb.position + movementDirection * Time.fixedDeltaTime * actualMovementSpeed);
        //transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime * actualMovementSpeed);
    }

    // rotate the player to face the direction of the camera
    // This is done in LateUpdate to ensure the camera has already updated its position and rotation
    // made with the help of A.I. in Visual Studio
    private void PlayerRotation()
    {
        if (cinemachineCameraTarget == null) return;

        // Nur die Y-Achse (Yaw) übernehmen
        Vector3 targetEuler = cinemachineCameraTarget.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, targetEuler.y, 0f);
    }

    private void LateUpdate()
    {
        PlayerRotation();
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    isGrounded = true;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    isGrounded = false;
    //}

   
}
