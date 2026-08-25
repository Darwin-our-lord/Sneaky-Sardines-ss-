using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public bool facingRight = true; //used to determine which way the player is facing

    private Rigidbody2D rb; // the rigidbody
    [SerializeField] float speed; // the speed of wich the player moves
    [SerializeField] float jumpHeight; // the height of wich the player jumps
    [SerializeField] bool grounded; // used to determine if the player is ón the ground, and therfore can jump
    SerializeField] float jumpCooldown = 1f; // the time between jumps - this fixes issues with double jumping on a single frame
    private float currentCooldownTime; // the timer itself

   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // fetchng the rigidbody
    }

    
    void FixedUpdate()
    {
        // making the timer go down, and stops when it's under 0 so it doesnt run forever
        if(currentCooldownTime >= 0) 
        {
            currentCooldownTime -= 0.1f;

        }

        // checks if W or Space is pressend while the player is on the ground and the cooldown has passed - if so, the player jumps
        if ((Keyboard.current.wKey.isPressed || Keyboard.current.spaceKey.isPressed) && grounded == true && currentCooldownTime <= 0)
        {
            rb.linearVelocityY += jumpHeight;
            currentCooldownTime = jumpCooldown;
            
        }

        // checks if D is pressed and if so, the player walks right
        if (Keyboard.current.dKey.isPressed)
        {
            rb.linearVelocityX = speed;
            facingRight = true;
        }
        // checks if D is pressed and if so, the player walks left

        if (Keyboard.current.aKey.isPressed)
        {
            rb.linearVelocityX = -speed;
            facingRight = false;
        }
        
        

    }
    
    // when the player ís colliding with something with the layer "Ground" and the normal of the contactpoint is pointing upwards (if the player is on top of the collider) -
    // grounded is set to true
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3 && collision.GetContact(0).normal == Vector2.up)
        {
            grounded = true;
         
        }
    }

    // When the player is exiting a collider with the layer "Ground", grounded is set to false
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 )
        {
            grounded = false;
        }
    }

}
