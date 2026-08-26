using UnityEngine;
using UnityEngine.InputSystem;

public class spinspin : GraftablePart
{
    float sub = -100;
    bool a = false;
    Rigidbody2D rb;
    PlayerMovement player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    protected override void OnAttach()
    {
        a = true;
        rb = GetComponentInParent<Rigidbody2D>();
        player = GetComponentInParent<PlayerMovement>();
        if (a == true)
        {
            transform.position += new Vector3(0.5f, 0.5f, 0);
            if (transform.position.y > sub)
            {
                sub = transform.position.y;
            }

        }
    }

     // Update is called once per frame
    void FixedUpdate()
    {
        if (a == true)
        {
            PlayerMovement player = GetComponentInParent<PlayerMovement>();
            Vector2 moveInput = player.input.actions["Move"].ReadValue<Vector2>();

            if (player.canJump)
            {
                sub = transform.position.y;
                transform.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
            }
            if (player.canJump == false)
            {
                if (moveInput.y < 0)
                {
                    sub = transform.position.y;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
                }
                else 
                {
                    sub = transform.position.y;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale = 0.2f;
                }
            }
        }

        
    }
             
}
