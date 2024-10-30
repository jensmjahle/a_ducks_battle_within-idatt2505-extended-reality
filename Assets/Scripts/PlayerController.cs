using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private float maxSpeed = 10f;
    private float acceleration = 10f; // Speed to increase to max speed
    private float deceleration = 10f; // Speed to decrease when stopping
    private PlayerInputActions playerControls;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;

    private InputAction move;
    private InputAction look;
    private InputAction fire;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearDamping = 0;
        rb.angularDamping = 0;
    }

    private void OnEnable() 
    {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }

    void Update()
    {
        // Read the input direction each frame
        moveDirection = move.ReadValue<Vector2>();
        lookDirection = look.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Determine the target velocity based on input and max speed
        Vector2 targetVelocity = moveDirection * maxSpeed;

        // Smoothly change current velocity based on acceleration or deceleration
        if (moveDirection != Vector2.zero)
        {
            // Accelerate towards the target velocity
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate to zero if there's no input
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // Apply the calculated velocity to the Rigidbody2D
        rb.linearVelocity = currentVelocity;

        Debug.Log("Current speed: " + currentVelocity.magnitude);
        Debug.Log($"Current Speed: {currentVelocity.magnitude}, Target Speed: {targetVelocity.magnitude}");

    }

    private void Fire(InputAction.CallbackContext context) 
    {

        Debug.Log("fired");
    }
}
