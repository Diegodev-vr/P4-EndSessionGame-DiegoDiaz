using UnityEngine;
// this let me use the new input system
using UnityEngine.InputSystem;

public class Player_1_Jump : MonoBehaviour
{
    ///// call the new input system I created in the input actions asset
    private Input_Actions_Platformer Input_Actions_Platformer;

    ///// declare variables for input actions and input values
    private InputAction jump;


    [Header("Rigibody")]
    ///// im uing phisics 2D so I need a Rigidbody2D to apply forces and control the jump
    [SerializeField] private Rigidbody2D rb;

    [Header("Jump Settings")]
    ////// the is the forc of the jump
    [SerializeField] private float jumpForce = 5f;
    ////// this cut the jump height when player realse the jump button early
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    ////// i have a sprite rendere with aniations and trigers
    [SerializeField] private Animator animator;

    [Header("Fall Settings")]
    ///// when the player is falling this make it faster
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    ///// this prevent the fall to go to fast 
    [SerializeField] private float maxFallSpeed = -10f;

    ///// state of the jump input
    private bool isJumpPressed = false;
    private bool isJumpHeld = false;
    private bool isJumpReleased = false;

    ///// prevent double jump in air
    private bool isjumping = false;

    [Header("Ground Detention Check")]
    ///// im using draw a gizmo in the scene view to see the ground check area
    ///// for that i need a layer mask to specify what is the ground and a transform to specify the position of the ground check
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    ///// Im using a box instead of a circle for the ground check
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);

    [Header("Coyote Time")]
    ///// this is the time after leaving the ground and the player can still jump
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    //////////// LOGIC ///////////

    /// Before game start initialize the input actions
    private void Awake()
    {
        Input_Actions_Platformer = new Input_Actions_Platformer();
    }

    private void OnEnable()
    {
        ///// get Inputs into the variable Jump
        jump = Input_Actions_Platformer.Player_1.Jump;

        ///// enable the input actions
        jump.Enable();

        ///// subscribe to the jump = button press = Callback
        jump.performed += onJumpPerformed;
        jump.canceled += onJumpCanceled;
    }

    private void OnDisable()
    {
        ///// disable the input actions
        jump.Disable();

        ///// unsubscribe from the jump
        jump.performed -= onJumpPerformed;
        jump.canceled -= onJumpCanceled;
    }
    private void Update()
    {
        ///// Check if player is grounded true or false with a function
        bool isGrounded = IsGrounded();

        ///// Bool to set animator transition grounded after jump
        animator.SetBool("IsGrounded", isGrounded);

        ////// Bool to set animator transition jumping after jump
        animator.SetBool("IsJumping", !isGrounded);
        ///// Handle coyote time 
        if (isGrounded)
        {
            ///// Refill coyote time when grounded
            coyoteTimeCounter = coyoteTime;
            ///// Reset jump state when grounded
            isjumping = false;
        }
        else
        {
            ///// Decrease coyote time counter in air
            coyoteTimeCounter -= Time.deltaTime; 
        }

        ///// Perform jump if all are true
        if (isJumpPressed && coyoteTimeCounter > 0f && !isjumping)
        {

            ///// trigger the Jump animation
            //animator.SetTrigger("Jump");
            //animator.SetFloat("Jumping", rb.linearVelocity.y);
            
            ///// Apply jump force by setting the vertical velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            ///// that way can not double jump in air
            isjumping = true;

            ///// Reset coyote time
            coyoteTimeCounter = 0f;
        }

        ///// Reset jump press flag (only valid for one frame)
        isJumpPressed = false;

        ///// Cut jump height if jump is released early (CLAMP jump up)
        if (isJumpReleased && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
        }

        ///// Reset jump release flag
        isJumpReleased = false;
    }

    ///// in fixed update I aply multiplyers
    private void FixedUpdate()
    {
        ///// strong gravity falling down
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        ///// when jump is released early (CLAMP jump up)
        else if (rb.linearVelocity.y > 0f && !isJumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        ///// Clamp fall speed to maxFallSpeed if not fall too fast
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }


    ///// OnJumpPerformed = Callback = Button Press = this method is ejecuted
    private void onJumpPerformed(InputAction.CallbackContext context)
    {
        // Check if the input action was performed (button pressed) = Callback
        if (context.performed)
        {

           // Debug.Log("Jump button pressed!");

           ///// Set flags to indicate jump was pressed and is being held
            isJumpPressed = true; 
            isJumpHeld = true;   
        }
    }
    ///// OnJumpCanceled = Callback = Button Release = this method is ejecuted
    private void onJumpCanceled(InputAction.CallbackContext context)
    {
        // Check if the input action was canceled (button released) = Callback
        if (context.canceled)
        {

            //Debug.Log("Jump button released!");

            ///// Set flags to indicate jump is no longer held and was released
            isJumpHeld = false;   
            isJumpReleased = true;
        }
    }
    ///// Checks if player is touching the ground using an overlapBox, im using this time a box instead of a circle
    ///// I need an anlge of 0f because the box is not rotated
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Draw the ground check box in Scene view
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
