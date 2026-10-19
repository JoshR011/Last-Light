// Team members: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song

using UnityEngine;
using UnityEngine.InputSystem;

// Converts player input into Rigidbody movement, jumping, and first-person camera rotation.
public class PlayerMovement : MonoBehaviour
{
    // Movement and horizontal-look settings, stored input values, and the player's Rigidbody.
    public float speed;
    public float rotationSpeed;
    private Vector2 movementValue;
    private float lookValue;
    private Rigidbody rb;
    private Collider playerCollider;

    // Jumping checks for ground directly beneath the player's collider.
    public float jumpForce;

    // Vertical-look settings and accumulated pitch control the camera independently of the body.
    public Transform cameraTransform;
    public float verticalLookSpeed;            
    private float verticalLookValue;           
    private float cameraPitch;                  

    private void Awake()
    {
        // Lock and hide the cursor for mouse look, then cache the Rigidbody for movement.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    public void OnMove(InputValue value)
    {
        // Store movement input scaled by speed for use during frame updates.
        movementValue = value.Get<Vector2>() * speed;
    }

    public void OnLook(InputValue value)
    {
        // Separate look input into horizontal body rotation and vertical camera rotation.
        lookValue = value.Get<Vector2>().x * rotationSpeed;
        verticalLookValue = value.Get<Vector2>().y * verticalLookSpeed;
    }

    public void OnJump(InputValue value)
    {
        // Only jump on a press, and do not jump again while rising.
        if (!value.isPressed || rb.velocity.y > 0.1f)
            return;

        Bounds bounds = playerCollider.bounds;
        bool onGround = Physics.Raycast(
            bounds.center, Vector3.down, out RaycastHit hit,
            bounds.extents.y + 0.1f, Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore) && hit.normal.y >= 0.5f;

        if (onGround)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Update()
    {
        // Apply movement force in the player's local axes, scaled by elapsed frame time.
        rb.AddRelativeForce(
            movementValue.x * Time.deltaTime,
            0,
            movementValue.y * Time.deltaTime);

        // Turn the player's body around its local vertical axis using horizontal look input.
        rb.AddRelativeTorque(0, lookValue * Time.deltaTime, 0);

        // Accumulate vertical look, limit the viewing angle, and apply it to the camera.
        cameraPitch -= verticalLookValue * Time.deltaTime;
        cameraPitch = Mathf.Clamp(cameraPitch, -80, 80);                     
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0); 
    }
}
