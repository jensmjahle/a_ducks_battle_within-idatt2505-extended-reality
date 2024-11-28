using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject projectilePrefab;
    public Transform firePoint;

    // Prefab management
    public GameObject currentPlayerPrefab;
    private GameObject activePlayerInstance;
    private Animator baseAnimator; // The base animator (always running)
    private Animator[] overlayAnimators; // Array to store overlay animators (can be 1 or 2)


    private PlayerPrefabManager prefabManager;


    private float maxSpeed = 10f;
    private float acceleration = 10f; // Speed to increase to max speed
    private float deceleration = 10f; // Speed to decrease when stopping
    private bool isShooting = false;
    private ColorVariant currentColorVariant;
    private WeaponType currentWeaponType;
    private string currentDirection = "down"; // "left", "right", "up", "down"
    private bool isMoving = false;
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

        prefabManager = prefabManager = GetComponent<PlayerPrefabManager>();

        currentColorVariant = ColorVariant.A; // Set the default color variant
        currentWeaponType = WeaponType.Pistol; // Set the default weapon type
      

        // Find the base animator (which always runs)
        if (currentPlayerPrefab != null)
        {
            baseAnimator = currentPlayerPrefab.GetComponentInChildren<Animator>(); // Find base animator in child objects
            // Find all child animators and store them (overlay or second overlay)
            overlayAnimators = currentPlayerPrefab.GetComponentsInChildren<Animator>();

            // Filter out the base animator from the overlay animators (assuming only one animator is for the base)
            overlayAnimators = System.Array.FindAll(overlayAnimators, animator => animator != baseAnimator);
        }
        // Initialize the player prefab
        SwapPrefab();

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
        fire.performed += OnFirePerformed;
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

        // Calculate the direction of movement
        Vector3 direction = lookDirection.normalized;

        // Velg retning basert p� den st�rste komponenten
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Bevegelse hovedsakelig horisontalt
           // animator.SetFloat("MoveX", Mathf.Abs(direction.x));
            //animator.SetFloat("MoveY", 0); // Nullstill Y for � unng� feilaktig animasjon

            if (direction.x > 0)
                transform.localScale = new Vector3(-1, 1, 1); // Speil p� x-aksen
            else
                transform.localScale = new Vector3(1, 1, 1); // Normal retning
        }
        else
        {
            // Bevegelse hovedsakelig vertikalt
            // call the prefab change here for the player
        }

        // Hvis spilleren ikke beveger seg, sett animasjonen til 0
        if (moveDirection == Vector2.zero)
        {
            // call idle animation here
        }
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

        
    }


    // Method to enable or disable overlay animators. This activates or deactivates the firing animation.
    public void SetOverlayActive(bool isActive)
    {
        foreach (var overlayAnimator in overlayAnimators)
        {
            if (overlayAnimator != null)
            {
                Debug.Log("Before setting, Animator enabled: " + overlayAnimator.enabled);
                overlayAnimator.enabled = isActive;
                Start(); // Not a good practice, but for mvp
                Debug.Log("After setting, Animator enabled: " + overlayAnimator.enabled);
            }
        }
    }

    // Wrapper method to match the required signature
    private void OnFirePerformed(InputAction.CallbackContext context)
  {
      Fire(); // Call the actual firing logic
  }

      private void Fire()
  {
      // Instantiate the projectile
      GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

      // Calculate the angle and set the rotation
      float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
      projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));


      // Apply velocity to the projectile
      Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
      if (rb != null)
      {
          rb.linearVelocity = lookDirection.normalized * projectile.GetComponent<Projectile>().speed;
      }
  }


    public void SwapPrefab()
    {
        if (prefabManager == null)
        {
            Debug.LogError("PlayerPrefabManager is not assigned to the PlayerController.");
            return;
        }

        // Use the current weapon type and color variant to select the appropriate prefab
        WeaponType currentWeapon = currentWeaponType;   
        ColorVariant currentColor = currentColorVariant;
        PlayerDirection newDirection;

        if (currentDirection == "left")
        {
            if (isMoving)
            {
                newDirection = PlayerDirection.Side_Walk;
            }
            else
            {
                newDirection = PlayerDirection.Side_Idle;
            }
        } else if (currentDirection == "right") {
            if (isMoving) {
                newDirection = PlayerDirection.Side_Walk;
            }
            else
            {
                newDirection = PlayerDirection.Side_Idle;
            }
        }
        else if (currentDirection == "up")
        {
            if (isMoving)
            {
                newDirection = PlayerDirection.Up_Walk;
            }
            else
            {
                newDirection = PlayerDirection.Up_Idle;
            }
        }
        else if (currentDirection == "down")
        {
            if (isMoving)
            {
                newDirection = PlayerDirection.Down_Walk;
            }
            else
            {
                newDirection = PlayerDirection.Down_Idle;
            }
        
        }
        else 
        {
                newDirection = PlayerDirection.Side_Idle;
                Debug.LogError("Invalid direction: " + currentDirection);
        }




            Debug.Log("Current weapon: " + currentWeapon + ", Current color: " + currentColor + ", Direction: " + newDirection);

        // Call the prefab manager to swap the prefab
        prefabManager.SwapPrefab(currentWeapon, currentColor, newDirection);
    }

    private void StartShooting(InputAction.CallbackContext context)
    {
        SetOverlayActive(true); // Enable the firing overlay animation

        Vector2 input = context.ReadValue<Vector2>();
        if (input.x > 0)
        {
            lookDirection = Vector2.right;    // L key
        }
        else if (input.x < 0)
        {
            lookDirection = Vector2.left; // J key
        }
        else if (input.y > 0)
        {
            lookDirection = Vector2.up;   // I key
        }
        else if (input.y < 0)
        {
            lookDirection = Vector2.down; // K key
        }

      if (!isShooting)
      {
          isShooting = true;
          shootingCoroutine = StartCoroutine(ShootContinuously());
      }
    }

    private void StopShooting()
    {
        SetOverlayActive(false); // Disable the firing overlay animation
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
            Fire();

            yield return new WaitForSeconds(0.2f); // Adjust the firing rate as needed
        }
    }

}
