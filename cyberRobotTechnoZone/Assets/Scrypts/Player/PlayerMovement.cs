using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] bool grounded;
    private float jumpCooldown = 1f;
    [SerializeField] private float currentCooldownTime;

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

        if (Keyboard.current.wKey.isPressed && grounded == true && currentCooldownTime <= 0)
        {
            rb.linearVelocityY += jumpHeight;
            currentCooldownTime = jumpCooldown;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            grounded = true;
            rb.linearVelocityY = 0f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            grounded = false;
        }
    }

}
