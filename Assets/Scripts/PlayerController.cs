using System.Collections;
using System.Linq;
using System;
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
    private bool isFacingRight = false;
    private ColorVariant currentColorVariant;
    private Coroutine shootingCoroutine;
    private PlayerInputActions playerControls;
    

    private Vector2 moveDirection = Vector2.zero;
   // private Vector2 lookDirection = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;

    private InputAction move;
    private InputAction look;
    private InputAction fire;


    // Events to notify when a value changes
    public event Action OnMovementChanged;
    public event Action OnLookDirectionChanged;
    public event Action OnWeaponChanged;

    // Properties to encapsulate the state and notify listeners when a change occurs
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            if (_isMoving != value)
            {
                _isMoving = value;
                OnMovementChanged?.Invoke(); // Notify listeners when movement state changes
            }
        }
    }
    private Vector2 _lookDirection = Vector2.zero;
    public Vector2 LookDirection
    {
        get => _lookDirection;
        set
        {
            if (_lookDirection != value)
            {
                Debug.Log("Look direction changed: " + value);
                _lookDirection = value;
                OnLookDirectionChanged?.Invoke(); // Notify listeners when look direction changes
            }
        }
    }

    private WeaponType _currentWeaponType = WeaponType.Pistol;
    public WeaponType CurrentWeaponType
    {
        get => _currentWeaponType;
        set
        {
            if (_currentWeaponType != value)
            {
                _currentWeaponType = value;
                OnWeaponChanged?.Invoke(); // Notify listeners when weapon changes
            }
        }
    }




    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {

        prefabManager = prefabManager = GetComponent<PlayerPrefabManager>();

        currentColorVariant = ColorVariant.A; // Set the default color variant
        _currentWeaponType = WeaponType.Pistol; // Set the default weapon type
        _lookDirection = Vector2.down; // Set the default look direction

        //currentPlayerPrefab = prefabManager.SwapPrefab(currentWeaponType, currentColorVariant, PlayerDirection.Down_Idle);

        SwapPrefab();

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
       

        rb = GetComponent<Rigidbody2D>();

        rb.linearDamping = 0;
        rb.angularDamping = 0;

        // Subscribe to events to trigger prefab swap when values change
        OnMovementChanged += SwapPrefab;
        OnLookDirectionChanged += SwapPrefab;
        OnWeaponChanged += SwapPrefab;

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

      //  Debug.Log("Look " + newLookDirection);
      //  Debug.Log("LOOK DIRECTION " + _lookDirection);

        if (newLookDirection != Vector2.zero)
        {
            LookDirection = newLookDirection.normalized;
            if (!isShooting)
            {
                isShooting = true;
            //    SetOverlayActive(true); // Enable the firing overlay animation
                shootingCoroutine = StartCoroutine(ShootContinuously());
            }
        }
        else
        {
            StopShooting();
          //  SetOverlayActive(false); // Disable the firing overlay animation    
        }

        // Set the movement state
        IsMoving = moveDirection != Vector2.zero;

       


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
                overlayAnimator.enabled = isActive;
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
      float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
      projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));


      // Apply velocity to the projectile
      Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
      if (rb != null)
      {
          rb.linearVelocity = _lookDirection.normalized * projectile.GetComponent<Projectile>().speed;
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
        WeaponType currentWeapon = _currentWeaponType;   
        ColorVariant currentColor = currentColorVariant;
        PlayerDirection newDirection;

        if (_lookDirection.x > 0 || _lookDirection.x < 0) { // Right
            newDirection = _isMoving ? PlayerDirection.Side_Walk : PlayerDirection.Side_Idle;
            if(_lookDirection.x > 0) isFacingRight = true;
            else isFacingRight = false;
        }
        else if (_lookDirection.y > 0)  { // Up
            newDirection = _isMoving ? PlayerDirection.Up_Walk : PlayerDirection.Up_Idle;
           isFacingRight = false;
        }
        else if (_lookDirection.y < 0) { // Down
            newDirection = _isMoving ? PlayerDirection.Down_Walk : PlayerDirection.Down_Idle;
            isFacingRight = false;
        }
        else { // Default when no movement or direction 
            newDirection = PlayerDirection.Down_Idle;
            isFacingRight = false;
        }


        // Call the prefab manager to swap the prefab
        currentPlayerPrefab = prefabManager.SwapPrefab(currentWeapon, currentColor, newDirection);

        currentPlayerPrefab.transform.localScale = new Vector3(1, 1, 1); // Reset the scale to default for the prefab
        transform.localScale = new Vector3(!isFacingRight ? 1 : -1, 1, 1); // Flip the player based on the direction

        // Find the base animator (which always runs)
        baseAnimator = currentPlayerPrefab.GetComponentInChildren<Animator>(); // Find base animator in child objects
            // Find all child animators and store them (overlay or second overlay)
            overlayAnimators = currentPlayerPrefab.GetComponentsInChildren<Animator>();

            // Filter out the base animator from the overlay animators (assuming only one animator is for the base)
            overlayAnimators = System.Array.FindAll(overlayAnimators, animator => animator != baseAnimator);

    }


    private void StartShooting(InputAction.CallbackContext context)
    {
        SetOverlayActive(true); // Enable the firing overlay animation

        Vector2 input = context.ReadValue<Vector2>();
        if (input.x > 0)
        {
            _lookDirection = Vector2.right;    // L key
        }
        else if (input.x < 0)
        {
            _lookDirection = Vector2.left; // J key
        }
        else if (input.y > 0)
        {
            _lookDirection = Vector2.up;   // I key
        }
        else if (input.y < 0)
        {
            _lookDirection = Vector2.down; // K key
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
