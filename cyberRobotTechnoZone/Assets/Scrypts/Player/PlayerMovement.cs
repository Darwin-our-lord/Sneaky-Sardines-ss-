using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public bool facingRight = true; //used to determine which way the player is facing

    private Rigidbody2D rb; // the rigidbody

    [SerializeField] float speed; // the speed of wich the player moves
    [SerializeField] float jumpHeight; // the height of wich the player jumps

    private bool grounded; // used to determine if the player is ón the ground, and therfore can jump

    [SerializeField] float jumpCooldown = 1f; // the time between jumps - this fixes issues with double jumping on a single frame
    private float currentCooldownTime; // the timer itself

    [SerializeField] float cyoteTime; // the time after the player leaves the ground, but still is able to jump
    private float currentCyoteTime; // the timer itself

    public bool canJump { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // fetchng the rigidbody
    }

    
    void FixedUpdate()
    {
        // making the cooldown timer go down, and stops when it's under 0 so it doesnt run forever
        if(currentCooldownTime >= 0) 
        {
            currentCooldownTime -= 0.1f;

        }

        if (grounded == false)
        {
            // making the cyote timer go down, and when it reaches zero, the player can no longer jump in the air
            if (currentCyoteTime >= 0)
            {
                currentCyoteTime -= 0.1f;

            }
            else
            {
                canJump = false;
            }
        }
        

        // checks if W or Space is pressend while the player is on the ground and the cooldown has passed - if so, the player jumps 
        if ((Keyboard.current.wKey.isPressed || Keyboard.current.spaceKey.isPressed) && canJump == true && currentCooldownTime <= 0)
        {
            Vector2 velocity = Vector2.zero;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);

            currentCooldownTime = jumpCooldown;
            
            grounded = false;
            canJump = false;
            
        }

        // checks if D is pressed and if so, the player walks right and is set to be facing right
        if (Keyboard.current.dKey.isPressed)
        {
            Vector2 velocity = Vector2.zero;
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, new Vector2(speed, rb.linearVelocityY),ref velocity, 0.1f);
            facingRight = true;
        }
     
        // checks if D is pressed and if so, the player walks left and is set to be facing left

        if (Keyboard.current.aKey.isPressed)
        {
            Vector2 velocity = Vector2.zero;
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, new Vector2(-speed, rb.linearVelocityY),ref velocity, 0.1f);
            facingRight = false;
        }

        if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed && Mathf.FloatToHalf(rb.linearVelocityX) != 0 && grounded == true)
        {
                rb.linearVelocityX *= 0.8f
        }

    }
    
    // when the player ís colliding with something with the layer "Ground" and the normal of the contactpoint is pointing upwards (if the player is on top of the collider) -
    // grounded is set to true
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3 && collision.GetContact(0).normal == Vector2.up && rb.linearVelocityY <= 0)
        {
            grounded = true;
            canJump = true;
         
        }
    }

    // When the player is exiting a collider with the layer "Ground", grounded is set to false
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 )
        {
            grounded = false;
            currentCyoteTime = cyoteTime;
        }
    }

}
