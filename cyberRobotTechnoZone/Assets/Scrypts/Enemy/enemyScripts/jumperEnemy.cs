using UnityEngine;

public class jumperEnemy : Enemy
{
    [SerializeField] float speed = 0.01f;
    GameObject player;
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FixedUpdate()
    {
        //jump
        if (player != null && (transform.position - player.transform.position).magnitude < 6f && (transform.position - player.transform.position).magnitude > 5f)
        {
            
            return;
        }

        //retreat or chase
        if (player != null && (transform.position - player.transform.position).magnitude < 15f)
        {
            Vector3 dir = (transform.position - player.transform.position).normalized;
            transform.position += dir * speed;

        }

    }
}
