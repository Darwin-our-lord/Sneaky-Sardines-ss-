using UnityEngine;

public class AcornEnemy : Enemy
{
    int dir = 1;

    [SerializeField] float speed = 0.01f; 
    [SerializeField] float edgeBuffer = 0.5f; //how close to the edge of the platform before turning around
    [SerializeField] LayerMask groundLayer; //what counts as ground for the enemy to walk on
    public void FixedUpdate()
    {
        //patrol
        Vector3 origin = transform.position + transform.right * dir * edgeBuffer;
        RaycastHit2D raycast = Physics2D.Raycast(origin, Vector2.down, 1.5f, groundLayer);

        if(raycast.collider == null)
        {
            TurnAround();
        }

        transform.position += transform.right * dir * speed; //move

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Floor") && collision.GetContact(0).normal.y > 0.5f)
        { 
            TurnAround();
        }
        if(collision.collider.CompareTag("Player"))
        {
            //player take dmg
        }
    }
    public void TurnAround()
    {
        dir *= -1; //turn around
    }
}
