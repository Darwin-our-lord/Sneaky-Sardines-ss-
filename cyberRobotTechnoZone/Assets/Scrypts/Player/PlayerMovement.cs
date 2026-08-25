using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public bool facingRight = true; //used to determine which way the player is facing

    private Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] bool grounded;
    private float jumpCooldown = 1f;
    [SerializeField] private float currentCooldownTime;

    ContactPoint2D contactPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        if(currentCooldownTime >= 0)
        {
            currentCooldownTime -= 0.1f;

        }

        if ((Keyboard.current.wKey.isPressed || Keyboard.current.spaceKey.isPressed) && grounded == true && currentCooldownTime <= 0)
        {
            rb.linearVelocityY += jumpHeight;
            currentCooldownTime = jumpCooldown;
            
        }

        if (Keyboard.current.dKey.isPressed)
        {
            rb.linearVelocityX = speed;
            facingRight = true;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            rb.linearVelocityX = -speed;
            facingRight = false;
        }
        
        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        contactPoint = collision.GetContact(0);
        if(collision.gameObject.layer == 3 && contactPoint.normal == Vector2.up)
        {
            grounded = true;
            //rb.linearVelocityY = 0f;
         
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 )
        {
            grounded = false;
        }
    }

}
