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
                float facingDirection = playerMovement.facingRight ? 1f : -1f; //determine the direction the player is facing (1 for right, -1 for left)

                transform.position = new Vector3(transform.parent.position.x+ facingDirection * 0.8f, transform.parent.position.y, transform.parent.position.z);
                transform.rotation = Quaternion.Euler(0, 0, facingDirection * 90f);
            }
            else
            {
                transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 0.7f, transform.parent.position.z);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (held && collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(1);
        }
    }



}
