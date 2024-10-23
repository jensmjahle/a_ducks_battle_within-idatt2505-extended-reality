using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public PlayerInputActions playerControls;

    Vector2 moveDirection = Vector2.zero;

    private InputAction move;
    private InputAction fire;

    private void Awake()
    {
      playerControls = new PlayerInputActions();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
      rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void Fire(InputAction.CallbackContext context) 
    {
      Debug.Log("fired");
    }
}
