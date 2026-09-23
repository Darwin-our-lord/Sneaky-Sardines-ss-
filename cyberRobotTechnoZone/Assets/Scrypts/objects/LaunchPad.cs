using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public GameObject player;
    public float fortniteLaunchPadForce = 0f;
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    Rigidbody2D rb;

    private bool canLaunch = true;

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

        playerAnimator.SetFloat("VerticalVelocity", 100f);
        playerAnimator.Play("Jump", 0, 0f);

        Invoke(nameof(ResetScript), 0.2f);
        Invoke(nameof(ResetCooldown), 0.5f);
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
