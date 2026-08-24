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
            if (playerRb.linearVelocity.magnitude > 0.1f)
            {
                float facingDirection = playerMovement.facingRight ? 1f : -1f; //determine the direction the player is facing (1 for right, -1 for left)
                transform.position = transform.parent.position += transform.right * facingDirection * 0.7f;
            }
            else
            {
                transform.position = transform.parent.position += transform.up * 0.7f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (held && collision.GetComponent<AcornEnemy>())
        {
            collision.GetComponent<AcornEnemy>().TurnAround();
        }
    }



}
