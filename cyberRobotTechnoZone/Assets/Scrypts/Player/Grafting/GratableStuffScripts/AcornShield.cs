using UnityEditor.Rendering;
using UnityEngine;

public class AcornShield : GraftablePart
{
    PlayerMovement playerMovement;
    Rigidbody2D playerRb;
    protected override void OnAttach()
    {
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
        playerRb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (held)
        {
            if (playerRb.linearVelocity.magnitude < 0.1f)
            {
                transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 2.5f, transform.parent.position.z);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                float facingDirection = playerMovement.facingRight ? 1f : -1f; //determine the direction the player is facing (1 for right, -1 for left)

                transform.position = new Vector3(transform.parent.position.x + facingDirection * 0.8f, transform.parent.position.y, transform.parent.position.z);
                transform.rotation = Quaternion.Euler(0, 0, facingDirection * -90f);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (held && collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 knockbackDirection;
            if (playerRb.linearVelocity.magnitude < 0.1f)
            {
                knockbackDirection = gameObject.transform.eulerAngles;
            }
            else
            {
                knockbackDirection = playerMovement.facingRight ? Vector3.right : Vector3.left;
            }

            
            if (collision.GetComponent<Enemy>() == null)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                collision.GetComponent<Enemy>().TakeKnockBack(20f, knockbackDirection);
            }
        }
    }



}
