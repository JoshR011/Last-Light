using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private Vector2 movementValue;
    private float lookValue;
    private Rigidbody rb;

    public float jumpForce;                     
    private bool onGround;                      

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>() * speed;
    }

    public void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>().x * rotationSpeed;
    }

    public void OnJump()                        
    {                                          
        if (onGround)                           
        {                                       
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
        }                                       
    }                                           

    void OnCollisionEnter(Collision other)      
    {                                          
        onGround = true;                        
    }                                          

    void OnCollisionExit(Collision other)       
    {                                           
        onGround = false;                       
    }                                          

    void Update()
    {
        rb.AddRelativeForce(
            movementValue.x * Time.deltaTime,
            0,
            movementValue.y * Time.deltaTime);

        rb.AddRelativeTorque(0, lookValue * Time.deltaTime, 0);
    }
}