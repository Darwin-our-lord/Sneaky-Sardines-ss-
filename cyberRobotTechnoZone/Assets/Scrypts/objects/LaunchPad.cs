using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public GameObject player;
    public float fortniteLaunchPadForce = 0f;
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    [SerializeField] Animator animator;
    Rigidbody2D rb;

    private bool canLaunch = true;

    [Header("Bounce Spawn")]
    [SerializeField] GameObject bouncePrefab;
    [SerializeField] Vector3 bounceSpawnOffset = Vector3.zero;
    [SerializeField] float bouncePrefabLifetime = 2f;


    void Start()
    {
        player = GameObject.Find("Player 1");
        rb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAnimator = player.GetComponent<Animator>();
    }

    void ManFlyver()
    {
        canLaunch = false;

        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForceY(fortniteLaunchPadForce, ForceMode2D.Impulse);

        playerMovement.grounded = false;
        playerMovement.enabled = false;

        animator.SetTrigger("Bounce");
        playerAnimator.SetFloat("VerticalVelocity", 100f);
        playerAnimator.SetTrigger("Jump");

        SpawnBouncePrefab();

        Invoke(nameof(ResetScript), 0.2f);
        Invoke(nameof(ResetCooldown), 0.5f);
    }

    void SpawnBouncePrefab()
    {
        if (bouncePrefab != null)
        {
            GameObject spawned = Instantiate(bouncePrefab, transform.position + bounceSpawnOffset, Quaternion.identity);
            if (bouncePrefabLifetime > 0f)
            {
                Destroy(spawned, bouncePrefabLifetime);
            }
        }
    }

    void ResetScript()
    {
        playerAnimator.SetFloat("VerticalVelocity", 100f);
        playerMovement.enabled = true;
    }

    void ResetCooldown()
    {
        canLaunch = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canLaunch && collision.gameObject == player)
        {
            ManFlyver();
        }
    }

    void Update()
    {

    }
}