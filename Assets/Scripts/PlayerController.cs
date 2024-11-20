using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject projectilePrefab;

    public Transform firePoint;
    private float maxSpeed = 10f;
    private float acceleration = 10f; // Speed to increase to max speed
    private float deceleration = 10f; // Speed to decrease when stopping
    private bool isShooting = false;
    private Coroutine shootingCoroutine;
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

        look = playerControls.Player.Look;
        look.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        fire.Disable();
    }

    void Update()
    {
        // Read the input direction each frame
        moveDirection = move.ReadValue<Vector2>();
        Vector2 newLookDirection = look.ReadValue<Vector2>();

        if (newLookDirection != Vector2.zero)
        {
            lookDirection = newLookDirection.normalized;
            if (!isShooting)
            {
                isShooting = true;
                shootingCoroutine = StartCoroutine(ShootContinuously());
            }
        }
        else
        {
            StopShooting();
        }
        Debug.Log("Lookdirection " + lookDirection);
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
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null) 
        {
            rb.linearVelocity = lookDirection.normalized * projectile.GetComponent<Projectile>().speed;
        }
        
        Debug.Log("fired");
    }

    private void StartShooting(InputAction.CallbackContext context)
    {
      Vector2 input = context.ReadValue<Vector2>();
      
      if (input.x > 0) lookDirection = Vector2.right;    // L key
      else if (input.x < 0) lookDirection = Vector2.left; // J key
      else if (input.y > 0) lookDirection = Vector2.up;   // I key
      else if (input.y < 0) lookDirection = Vector2.down; // K key

      Debug.Log("Look direction " + lookDirection);
      if (!isShooting)
      {
          isShooting = true;
          shootingCoroutine = StartCoroutine(ShootContinuously());
      }
    }

    private void StopShooting()
    {
        isShooting = false;
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootContinuously()
    {
        while (isShooting)
        {
            // Instantiate and shoot the projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = lookDirection.normalized * projectile.GetComponent<Projectile>().speed;
            }

            yield return new WaitForSeconds(0.2f); // Adjust the firing rate as needed
        }
    }

}
