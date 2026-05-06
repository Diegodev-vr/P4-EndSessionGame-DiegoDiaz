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

    ///// call the new input system I created in the input actions asset
    private Input_Actions_Platformer Input_Actions_Platformer;

    ///// declare variables for input actions and input values
    private InputAction move;
    private Vector2 moveInput;

    ///// variable to track if the player is currently knocked back
    private bool isKnockedBack = false;

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


        ///// enable the input actions
        move.Enable();
    }

    private void OnDisable()
    {
        ///// disable the input actions
        move.Disable();
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
        ///// if the player is currently knocked back, skip movement logic to prevent interference with knockback effect
        if (isKnockedBack) return;

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

    ///// method to apply knockback force to the player when hit by a damage zone
    public void ApplyKnockback(Vector2 force)
    {
        isKnockedBack = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        Invoke(nameof(ResetKnockback), 0.2f);
    }

    ///// method to reset the knockback state after a short duration, allowing the player to regain control
    private void ResetKnockback()
    {
        isKnockedBack = false;
    }

}
