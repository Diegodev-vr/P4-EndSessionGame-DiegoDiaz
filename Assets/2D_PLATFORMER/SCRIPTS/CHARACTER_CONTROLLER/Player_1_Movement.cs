using UnityEngine;
// this let me use the new input system
using UnityEngine.InputSystem;

public class Player_1_Movement : MonoBehaviour
{
    ///// I choosed rigid body for the player movement becaue Jump has Rigidbody also
    ///// physics based movement is more consistent and less buggy than transform based movement
    ///// physics based movement also allows for better collision detection and response
    ///// keep physics consistent between codes - linearvelocity

    ///// Declare variables for movement parameters and components
    [Header("Movement")]
    [SerializeField] private float moveMaxSpeed = 5f;
    [SerializeField] private float accelerationMultiplier = 10f;
    [SerializeField] private float decelerationMultiplier = .9f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;


    ///// call the new input system I created in the input actions asset
    private Input_Actions_Platformer Input_Actions_Platformer;

    ///// declare variables for input actions and input values
    private InputAction move;
    private InputAction fire;
    private Vector2 moveInput;


    //////////// LOGIC ///////////

    /// Before game start initialize the input actions
    private void Awake()
    {
        Input_Actions_Platformer = new Input_Actions_Platformer();
    }

    private void OnEnable()
    {
        ///// get Inputs into the variables
        move = Input_Actions_Platformer.Player_1.Move;
        fire = Input_Actions_Platformer.Player_1.Fire;

        ///// enable the input actions
        move.Enable();
        fire.Enable();

        ////// subscribe to the fire input action performed event to call the Shoot method when the fire button is pressed
        fire.performed += Shoot;

    }

    private void OnDisable()
    {
        ///// disable the input actions
        move.Disable();
        fire.Disable();

        ////// unsubscribe from the fire input action performed event to prevent memory leaks
        fire.performed -= Shoot;


    }

    private void Update()
    {
        ///// read the movement input VALUE every frame = Polling
        moveInput = move.ReadValue<Vector2>();

        ///// Flip sprite based on direction
        if (moveInput.x > 0)
            transform.localScale = Vector3.one;
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
    private void FixedUpdate()
    {
        ///// if player is pressing move = accelerate, else = decelerate
        if (moveInput.x != 0)
        {
            ///// set the walk animation trigger
            animator.SetTrigger("Walk");
            ///// add acceleration to the player's horizontal velocity based on input and time
            rb.linearVelocity = new Vector2(rb.linearVelocity.x + moveInput.x * accelerationMultiplier * Time.fixedDeltaTime, rb.linearVelocity.y);
        }
        else
        {
            ///// set the idle animation trigger
            animator.SetTrigger("Idle");
            ///// apply deceleration to the player's horizontal velocity by multiplying it with a deceleration multiplier
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * decelerationMultiplier, rb.linearVelocity.y);
        }
        ///// Clamp the player's horizontal speed to the maximum speed -moveMaxSpeed = left = -5 , moveMaxSpeed = right = 5
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -moveMaxSpeed, moveMaxSpeed), rb.linearVelocity.y);
    }

    ////// method to shoot a bullet when the fire input action is performed
    private void Shoot(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            bulletScript.Initialize(direction);
        }
    }

}
