using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AcornShield : GraftablePart
{
    public static bool AcornShieldw = false;
    PlayerMovement playerMovement;
    Rigidbody2D playerRb;

    [SerializeField] AudioClip AttachSound; // The sound of the player attaching the shield
    [SerializeField] AchievementScreenManager achievementScreenManager; // Reference to the AchievementScreenManager script on canvas

    PlayerManager playerManager;

    private InputActionMap actionMap;
    private InputAction blockAction;
    [SerializeField] float blockDelay = 1;
    [SerializeField] float blockDuration = 0.3f;
    bool canBlock = true;
    bool blocking = false;

    void Awake()
    {
        actionMap = new InputActionMap("Player");

        blockAction = actionMap.AddAction("Block");
        blockAction.AddBinding("<Keyboard>/k");
    }

    private void Start()
    {
        AcornShieldw = false;
        achievementScreenManager = FindAnyObjectByType<AchievementScreenManager>();
    }
    protected override void OnAttach()
    {
        AcornShieldw = true;
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
        playerRb = transform.parent.GetComponent<Rigidbody2D>();
        AudioSource.PlayClipAtPoint(AttachSound, transform.position, 10f);
        Debug.Log(achievementScreenManager);
        achievementScreenManager = FindAnyObjectByType<AchievementScreenManager>();
        achievementScreenManager.UnlockNewAbility("AcornShield");

    }

    private void FixedUpdate()
    {
        if (held)
        {
            if (playerManager == null) playerManager = FindAnyObjectByType<PlayerManager>();
            if (blockAction.IsPressed() && canBlock)
            {
                canBlock = false;

                blocking = true;
                playerManager.shieldInvincibilty = true;
                GetComponent<SpriteRenderer>().enabled = true;

                StartCoroutine(WaitAndStopBlock());
                StartCoroutine(WaitAndAllowBlock());
            }


        }
    }
    IEnumerator WaitAndStopBlock()
    {
        yield return new WaitForSeconds(blockDuration);
        blocking = false;
        playerManager.shieldInvincibilty = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    IEnumerator WaitAndAllowBlock()
    {
        yield return new WaitForSeconds(blockDelay);
        canBlock = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!blocking) return;
        
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
