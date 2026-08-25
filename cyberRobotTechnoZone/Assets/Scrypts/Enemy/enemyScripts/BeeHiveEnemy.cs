using UnityEngine;

public class BeeHiveEnemy : Enemy
{
    GameObject player;
    float shootCooldownTimer = 2f;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] GameObject beePrefab;
    [SerializeField] LayerMask layerMask;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        shootCooldownTimer -= Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position).normalized, 10, layerMask); //check for walls between the player and the enemy
        if (shootCooldownTimer <= 0 && hit.collider != null && hit.collider.CompareTag("Player")) 
        {
            //shoot bee
            GameObject bee = Instantiate(beePrefab, transform.position, Quaternion.identity);
            shootCooldownTimer = shootCooldown;
        }
    }
}
