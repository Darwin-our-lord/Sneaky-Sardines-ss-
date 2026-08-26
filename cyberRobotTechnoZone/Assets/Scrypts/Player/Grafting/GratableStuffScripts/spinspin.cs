using UnityEngine;
using UnityEngine.InputSystem;

public class spinspin : GraftablePart
{
    float sub = -100;
    bool a = false;
    Rigidbody2D rb;
    PlayerMovement player;
    private bool dontSpin;
    
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
            if (Keyboard.current.sKey.isPressed)
            {
                dontSpin = true;
            }
            else
            {
                dontSpin = false;
            }
          
            if (player.canJump)
            {
                sub = transform.position.y;
                transform.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
            }
            if (player.canJump == false)
            {
                if (sub > transform.position.y && dontSpin == false)
                {
                    sub = transform.position.y;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale = 0.2f;
                }
                else if (sub <= transform.position.y || dontSpin == true)
                {
                    sub = transform.position.y;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
                }
            }
        }

        
    }
             
}
