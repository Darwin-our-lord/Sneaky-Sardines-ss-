using UnityEngine;

public class spinspin : GraftablePart
{
    float wait = 0.5f;
    float time = 0;
    float sub = -100;
    bool a = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    protected override void OnAttach()
    {
        a = true;
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
            if (player.canJump)
            {
                sub = transform.position.y;
                transform.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
            }
            if (player.canJump == false)
            {
                if (sub > transform.position.y)
                {
                    sub = transform.position.y;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale = 0.2f;
                }
                else if (sub <= transform.position.y)
                {
                    sub = transform.position.y;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
                }
            }
        }

        
    }
             
        }
